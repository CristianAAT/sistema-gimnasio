#pragma checksum "C:\Users\abcustoms\OneDrive\Escritorio\Cristian\CAAT Proyectos Portafolio\Sistema gestion de GYM\SistemaGYM\SistemaGYM\Views\Reportes\Reportes.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "39d89e3cad9d9a0e887754e19893a927078d18ec"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Reportes_Reportes), @"mvc.1.0.view", @"/Views/Reportes/Reportes.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\abcustoms\OneDrive\Escritorio\Cristian\CAAT Proyectos Portafolio\Sistema gestion de GYM\SistemaGYM\SistemaGYM\Views\_ViewImports.cshtml"
using SistemaGYM;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\abcustoms\OneDrive\Escritorio\Cristian\CAAT Proyectos Portafolio\Sistema gestion de GYM\SistemaGYM\SistemaGYM\Views\_ViewImports.cshtml"
using SistemaGYM.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"39d89e3cad9d9a0e887754e19893a927078d18ec", @"/Views/Reportes/Reportes.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"82c0ecfb7aa6b8120751389b90f58c0600ce56bf", @"/Views/_ViewImports.cshtml")]
    public class Views_Reportes_Reportes : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onload", new global::Microsoft.AspNetCore.Html.HtmlString("Reportes.GetMonths(); Reportes.Fecha(); Reportes.Buscar();"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\abcustoms\OneDrive\Escritorio\Cristian\CAAT Proyectos Portafolio\Sistema gestion de GYM\SistemaGYM\SistemaGYM\Views\Reportes\Reportes.cshtml"
  
    ViewData["Title"] = "Reportes";
    Layout = "~/Views/Shared/_Layout.cshtml";


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "39d89e3cad9d9a0e887754e19893a927078d18ec4026", async() => {
                WriteLiteral(@"

        <h1 class=""text-center""><i class=""fas fa-chart-bar""></i>   Reportes</h1>
        <div class=""space-top-sm"">
            <div class=""row"">
                <div class=""col-3"">
                    <Select id=""filtrarMes"" placeholder=""Buscar"" class=""form-control"" onkeyup=""Reportes.Buscar();"">
                    </Select>
                </div>
                <div class=""col-3"">
                    <Select id=""filtrarAnio"" placeholder=""Buscar"" class=""form-control"" onkeyup=""Reportes.Buscar();"">
                    </Select>
                </div>
                <div class=""col-3"">
                    <a onclick=""Reportes.Buscar()"" class=""btn btn-info"">Buscar</a>
                </div>
            </div>
            <!--Tabla de ventas-->
            <div id=""tabla"" class=""space-top-lg space-top-lg"">
                <center><h3><strong>Reporte de ventas mensual</strong></h3></center>
                <div>
                    <table class=""table table-hover"">
                       ");
                WriteLiteral(@" <thead>
                            <tr>
                                <th scope=""col"">
                                    Fecha
                                </th>
                                <th scope=""col"">
                                    Cliente
                                </th>
                                <th scope=""col"">
                                    Producto
                                </th>
                                <th scope=""col"">
                                    Costo
                                </th>
                            </tr>
                        </thead>
                        <tbody id=""ResultTable"">
                        </tbody>
                    </table>
                    <div id=""InfoTable""></div>
                </div>
            </div>
            <div>
                <a onclick=""PrintThisDiv('tabla');"" class=""btn btn-outline-success"">Obtener reporte PDF</a>
            </div>
            <!--Grafica-");
                WriteLiteral(@"->
            <div class=""space-top-lg space-top-lg"" id=""grafica"">
                <center><h3><strong>Grafica de ventas del ultimo año</strong></h3></center>
                <div id=""Ingresos""></div>
            </div>
            <div>
                <a onclick=""PrintThisDiv('grafica');"" class=""btn btn-outline-success"">Obtener reporte PDF</a>
            </div>
        </div>
    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
