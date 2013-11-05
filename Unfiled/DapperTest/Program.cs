using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DapperTest
{
    //This one needs to be converted to a simple program that CRUDs a table in an SDF using Dapper.
    class Program
    {
        public sealed class Timecard
        {
            public Guid Id { get; set; }
            public Guid BartenderId { get; set; }
            public DateTime ClockIn { get; set; }
            public DateTime ClockOut { get; set; }
        }

        static void Main(string[] args)
        {
            var oldFile = System.IO.Path.Combine(Environment.CurrentDirectory, "PoSh.sdf");
            var newFile = System.IO.Path.Combine(Environment.CurrentDirectory, "PoShNew.sdf");
            using(var oldConn = new SqlCeConnection("Data Source=" + oldFile))
            using(var newConn = new SqlCeConnection("Data Source=" + newFile))
            {
                oldConn.Open();
                newConn.Open();

                newConn.Execute("delete from timecard");
                newConn.Execute("delete from bartender");

                var countBefore = newConn.Query<int>("select count(id) from bartender").Single();

                var bts = oldConn.Query("select * from bartender");
                newConn.Execute("insert into bartender(id, name, hire, fire, pin) values(@Id,@Name,@Hire,@Fire,@Pin)", bts);

                var countAfter = newConn.Query<int>("select count(id) from bartender").Single();

                bool worked = countAfter > countBefore;

                var last = newConn.Query<DateTime>("select max(ClockOut) from timecard").Single();

                var newTimecards = oldConn.Query<Timecard>("Select * from timecard where ClockOut > @LastClockOut", new
                    {
                        LastClockOut = last
                    }
                );

                newConn.Execute("insert into timecard(Id, BartenderId, ClockIn, ClockOut) values (@Id, @BartenderId, @ClockIn, @ClockOut)", newTimecards);

                var result = newConn.Query("Select * from timecard where ClockOut > @LastClockOut", new
                {
                    LastClockOut = last
                }
                );
                foreach(var item in result)
                    Console.WriteLine("{0} {1} {2}", item.BartenderId, item.ClockIn, item.ClockOut);
            }
            Console.Read();
        }
    }
}
