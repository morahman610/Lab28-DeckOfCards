using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lab28_DeckofCards.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpWebRequest WR = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            WR.UserAgent = ".NET Framework Test Client";



            HttpWebResponse Response;


            try
            {
                Response = (HttpWebResponse)WR.GetResponse();

            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string DeckData = reader.ReadToEnd();

            JToken DeckID;
            try
            {
                JObject JsonData = JObject.Parse(DeckData);
                DeckID = JsonData["deck_id"];
                ViewBag.DeckID = DeckID;
            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }



            HttpWebRequest DC = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/" + DeckID +"/draw/?count=5");
            DC.UserAgent = ".Net Framework Test Client";

            HttpWebResponse DCResponse;

            try
            {
                DCResponse = (HttpWebResponse)DC.GetResponse();

            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (DCResponse.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = DCResponse.StatusCode;
                ViewBag.ErrorDescription = DCResponse.StatusDescription;
                return View();
            }

            StreamReader DCreader = new StreamReader(DCResponse.GetResponseStream());
            string DCDeckData = DCreader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(DCDeckData);
                ViewBag.DeckID = JsonData["deck_id"];
                ViewBag.Suit = JsonData["cards"]["suit"];
                ViewBag.Value = JsonData["cards"]["value"];
                ViewBag.Image = JsonData["cards"]["image"];

            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}