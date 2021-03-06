﻿using System.Web;
using System.Web.Optimization;

namespace MyPhotos
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/modernizr.custom.97116.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularApp").Include(
                      "~/AngularApp/app/myPhotosApp.js",
                      "~/AngularApp/controllers/galleriesListController.js",
                      "~/AngularApp/controllers/galleryViewController.js",
                      "~/AngularApp/services/contentService.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/iPhoneApp").Include(
                "~/iPhoneApp/app/iPhoneApp.js",
                "~/iPhoneApp/controllers/galleriesListController.js",
                "~/iPhoneApp/controllers/galleryViewController.js",
                "~/iPhoneApp/controllers/InfoModalController.js",
                "~/iPhoneApp/services/contentService.js"));

            bundles.Add(new StyleBundle("~/Content/iPhone").Include(
                    "~/Content/bootstrap.css", 
                    "~/iPhoneApp/css/iPhoneApp.css"));

            bundles.Add(new ScriptBundle("~/bundles/mobileApp").Include(
                    "~/mobileApp/app/mobileApp.js",
                    "~/mobileApp/controllers/galleriesListController.js",
                    "~/mobileApp/controllers/galleryViewController.js",
                    "~/mobileApp/controllers/InfoModalController.js",
                    "~/mobileApp/services/contentService.js"));

            bundles.Add(new StyleBundle("~/Content/mobile").Include(
                    "~/Content/bootstrap.css",
                    "~/mobileApp/css/mobileApp.css"));

            bundles.Add(new ScriptBundle("~/bundles/windowsPhoneApp").Include(
                    "~/windowsPhoneApp/app/windowsPhoneApp.js",
                    "~/windowsPhoneApp/controllers/galleriesListController.js",
                    "~/windowsPhoneApp/controllers/galleryViewController.js",
                    "~/windowsPhoneApp/controllers/InfoModalController.js",
                    "~/windowsPhoneApp/services/contentService.js"));

            bundles.Add(new StyleBundle("~/Content/windowsPhone").Include(
                    "~/Content/bootstrap.css",
                    "~/windowsPhoneApp/css/windowsPhoneApp.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
