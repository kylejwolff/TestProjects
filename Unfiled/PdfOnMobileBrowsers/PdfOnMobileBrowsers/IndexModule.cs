namespace PdfOnMobileBrowsers
{
    using System.IO;
    using Nancy;

    public class IndexModule : NancyModule
    {
        private const string _type = "application/pdf";
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["index"];
            };
            Get["/approved/(?<filename>.*pdf)"] = parameters =>
            {
                return Response.FromStream(new MemoryStream(Properties.Resources.Approved), _type);
            };
            Get["/pdfobj"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["pdfobj"];
            };
            Get["/objectwithembed"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["objectwithembed"];
            };
            Get["/objectwithalt"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["objectwithalt"];
            };
            Get["/objectonly"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["objectonly"];
            };
            Get["/embedonly"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["embedonly"];
            };
            Get["/iframe"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["iframe"];
            };
            Get["/iframetofile"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["iframetofile"];
            };
            Get["/all"] = parameters =>
            {
                ViewBag.BaseUrl = Request.Url.SiteBase;
                return View["all"];
            };
        }
    }
}