using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindStaticallyBlown
{
    //trying to repro some weird behavior
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Claim.Unregistered == null ? "WHAT THE EVERLOVING HELL" : Claim.Unregistered);
            Console.ReadLine();
        }
    }


    public static class Claim
    {
        /// <summary>
        /// Converts a list of claims to a claims string
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToClaimsString(this IEnumerable<string> target)
        {
            if(target == null)
                return Unregistered;
            var matching = target.Where(x => AllClaims.Contains(x)).ToArray();
            if(!matching.Any())
                return Unregistered;
            return string.Join(",", matching);
        }
        /// <summary>
        /// Converts a claims string to a list of claims.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToClaims(this string target)
        {

            if(string.IsNullOrWhiteSpace(target))
                return UnregisteredClaims;
            var temp = target.Split(',').Where(x => AllClaims.Contains(x)).ToArray();
            if(temp.Any())
                return temp;
            return UnregisteredClaims;
        }
        /// <summary>
        /// Default claims for an unregistered user
        /// </summary>
        public static readonly IEnumerable<string> UnregisteredClaims = new List<string> { Unregistered }.AsReadOnly();
        /// <summary>
        /// Default claims for a new user.
        /// </summary>
        public static readonly IEnumerable<string> NewUserClaims = new List<string> { NewUser, Registered }.AsReadOnly();

        private static readonly string[] AllClaims = new string[] 
        { 
            Unregistered,
            Registered,
            NewUser,
            Moderator,
            Administrator,
            Owner
        };
        /// <summary>
        /// The user is a new user to the system
        /// </summary>
        public static readonly string NewUser = "NEWB";
        /// <summary>
        /// The user is browsing anonymously.
        /// </summary>
        public static readonly string Unregistered = "WHODAFUCKAREYOU";
        /// <summary>
        /// User is registered.  Unknown users are referenced by <see cref="Users.Unknown"/>.
        /// </summary>
        public static readonly string Registered = "GROWLMEBITCH";
        /// <summary>
        /// User can bully other users in a limited fashion.
        /// </summary>
        public static readonly string Moderator = "PEONDERPER";
        /// <summary>
        /// User is employed to bully moderators and succor users when they are bullied.
        /// </summary>
        public static readonly string Administrator = "KINGDERPER";
        /// <summary>
        /// WE RULE YOU.  BITCH.  
        /// </summary>
        public static readonly string Owner = "LORDHIGHDERPAMIGHTY";
    }
}
