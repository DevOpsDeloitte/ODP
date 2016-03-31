<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Eval.aspx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Evaluation" %>

<%@ Register Src="~/Evaluation/Controls/EvaluationControl.ascx" TagPrefix="uc1" TagName="EvaluationControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Evaluation Form</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:EvaluationControl runat="server" ID="EvaluationControl" />


<%--           <script type="text/javascript">
               var menuRight = document.getElementById('cbp-spmenu-s2'),
                body = document.body;
                
                showRight.onclick = function() {
                  classie.toggle( this, 'active' );
                  classie.toggle( menuRight, 'cbp-spmenu-open' );
                  return false;
                };

                showRightPushed.onclick = function() {
                  classie.toggle( this, 'active' );
                  classie.toggle( menuRight, 'cbp-spmenu-open' );
                  classie.remove( menuRight, 'expand' );
                  return false;
                };

                $(window).on('load resize', function(){
                    $('.cbp-spmenu').height($(this).height());
                });

              </script>

              <script type="text/javascript">
                  $(document).ready(function () {

                    $(".comments-display").show();

                    $(".tabs-menu a").click(function(event) {
                        event.preventDefault();
                        $(this).parent().addClass("current");
                        $(this).parent().siblings().removeClass("current");
                        var tab = $(this).attr("href");

                        $(".tab-content").not(tab).removeClass("current");
                        $(tab).addClass("current");
                        //$(".tab-content").not(tab).css("display", "none");
                        //$(tab).fadeIn();

                    });

                    $(".expand-comments").click(function(event) {
                      event.preventDefault();
                      $(this).parent().parent().toggleClass("expand");
                    });

                });                
              </script>--%>


</asp:Content>
