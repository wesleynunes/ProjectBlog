using System.Web;
using System.Web.Optimization;

namespace ProjectBlog
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/tinymce/tinymce.js",
                        "~/Scripts/tinymce/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/PagedList.css",
                      "~/Content/modern-business.css"));

            //Renderização de estilo para o Layout painel
            bundles.Add(new StyleBundle("~/Content/panel").Include(
                      "~/Content/panel/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/panel/vendor/font-awesome/css/font-awesome.min.css",                    
                      "~/Content/panel/vendor/datatables/dataTables.bootstrap4.css",
                      "~/Content/panel/css/sb-admin.css",
                      "~/Content/PagedList.css"
                      ));

            //Renderização de script .js para layout painel
            bundles.Add(new ScriptBundle("~/bundles/panel").Include(
                      "~/Content/panel/vendor/jquery/jquery.min.js",
                      "~/Content/panel/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Content/panel/vendor/jquery-easing/jquery.easing.min.js",
                      "~/Content/panel/vendor/chart.js/Chart.min.js",
                      "~/Content/panel/vendor/datatables/jquery.dataTables.js",
                      "~/Content/panel/vendor/datatables/dataTables.bootstrap4.js",
                      "~/Content/panel/js/sb-admin.min.js",
                      "~/Content/panel/js/sb-admin-datatables.min.js",
                      "~/Content/panel/js/sb-admin-charts.min.js"
                      ));

            //reduz a quantidade de arquivos js e css otimizando a aplicação
            // setado com false desabilita para fazer debug
            BundleTable.EnableOptimizations = false;
        }
    }
}
