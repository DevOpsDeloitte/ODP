﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluationControl.ascx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Controls.EvaluationControl" %>


	<!-- CSS
  ================================================== -->
	<link rel="stylesheet" href="../styles/base.css">
	<link rel="stylesheet" href="../styles/skeleton.css">
	<link rel="stylesheet" href="../styles/layout.css">
    <link rel="stylesheet" href="../styles/alertify.css">    
    <link rel="stylesheet" href="../styles/line/tax.css">
    <link href="../styles/evaluation.css" rel="stylesheet" />

	<!-- Favicons
	================================================== -->
	<link rel="shortcut icon" href="../images/favicon.ico">
	<link rel="apple-touch-icon" href="../images/apple-touch-icon.png">
	<link rel="apple-touch-icon" sizes="72x72" href="../images/apple-touch-icon-72x72.png">
	<link rel="apple-touch-icon" sizes="114x114" href="../images/apple-touch-icon-114x114.png">
<script type="text/javascript">
   // $scope.mdata.formmode = "<%= FormMode %>"
</script>

<div class="container" ng-controller="ODPFormCtrl">

    <div class="sixteen columns header"> 
        <h1 class="center">Prevention Taxonomy Form</h1>
        <span class="subtitle">CHECK ALL THAT APPLY IN EACH COLUMN (TOPICS ARE NOT MUTUALLY EXCLUSIVE)</span>
        <span class="subtitle">See accompanying protocol for definitions and examples</span>
    </div>

    <div class="subnav">
        <ul>
            <!--<li><a href="#study-focus">Study Focus</a></li>
            <li><a href="#entities-studied">Entities Studied</a></li>
            <li><a href="#study-setting">Study Setting</a></li>
            <li><a href="#population-focus">Population focus</a></li>
            <li><a href="#study-design-purpose">Study Design/Purpose</a></li>
            <li><a href="#prevention-research-category">Prevention Research Category</a></li>-->
            <!--<li><a class="button" href="#" id="confirmX" ng-click="processForm()" ng-disabled="{{1 == 1}}">Save</a></li>-->
            <li><input class="button yes" type="button" id="printButton" value="Print Abstract" ng-click="printAbstract()" /></li>
            <li ng-show="showResetButton"><input class="button yes" type="button" id="resetButton" value="Reset" ng-click="resetFormStart()" /></li>
            <li ng-show="showComparisonButton"><input class="button yes" type="button" id="comparisonButton" value="Start Comparison" ng-click="startComparison()" /></li>
            <li ng-show="!showComparisonButton && mode.indexOf('Comparison') != -1"><input class="button no" type="button" id="disabledcomparisonButton" value="Start Comparison" /></li>
            <li ng-show="showConsensusButton"><input class="button yes" type="button" id="consensusButton" value="Start Consensus" ng-click="startConsensus()" /></li>
            <li ng-show="!showConsensusButton && mode.indexOf('Consensus') != -1"><input class="button no" type="button" id="disabledconsensusButton" value="Start Consensus" /></li>
            <li ng-show="showSaveButton"><input class="button yes" type="button" id="saveButton" value="Save" ng-click="processForm()" ng-disabled="disallowSave" /></li>
            <li ng-show="!showSaveButton"><input class="button no" type="button" id="disabledSaveButton" value="Save"/></li>
            <%--<li ng-show="mode.indexOf('Consensus') != -1">Users Unable to Code : <%= unableCoders %></li>
            <li ng-show="mode.indexOf('Comparison') != -1">Teams Unable to Code : <%= unableCoders %></li>
            <li><input type="checkbox" name="unabletocode" id="unabletocode" ng-model="mdata.unabletocode"  ng-disabled="mdata.displaymode=='View'" /><label>Unable to Code</label></li>
            <li>
            <div ng-show="mdata.unabletocode && mdata.displaymode!='View'">
                <input type="text" id="superusername" name="superusername" ng-model="mdata.superusername"  placeholder="supervisor username"/>
                <input type="password" id="superpassword" name="superpassword" ng-model="mdata.superpassword"  placeholder="supervisor password"/>
            </div>
            
            </li>--%>
        </ul>
    </div>
    <div id="odpforms" name="x" ng-cloak>
    <div class="sixteen columns"> 
            <div>



        <div>
            <h2>{{postmessages}}</h2>
            <h2 class="errormessages">{{ errormessagesdisplay }}</h2>
        </div>

    </div>
        <input type="hidden" name="submissionid" value="<%= SubmissionID %>" />
        <input type="hidden" name="mode" id="mode" value="<%= FormMode %>" ng-model="mdata.formmode" />
        <input type="hidden" name="displaymode" id="displaymode" value="<%= DisplayMode %>" ng-model="mdata.displaymode" />  
        <input type="hidden" name="userid" id="userid" value="<%= UserId %>" ng-model="mdata.userid" />
        <input type="hidden" name="submissiontypeid" id="submissiontypeid" value="<%= SubmissionTypeId %>" ng-model="mdata.submissiontypeid" />
        <input type="hidden" name="abstractid" id="abstractid" value="<%= AbstractID %>" ng-model="mdata.abstractid" />
        <input type="hidden" name="showc" id="showc" value="<%= showConsensusButton %>" ng-model="mdata.showconsensusbutton" />
        <input type="hidden" name="showcomp" id="showcomp" value="<%= showComparisonButton %>" ng-model="mdata.showcomparisonbutton" />
        <input type="hidden" name="isunable" id="isunable" value="<%= isChecked %>" ng-model="mdata.isunable" />
        <!-- not need just a placholder for now -->
        <input type="hidden" name="evaluationid" value="<%= EvaluationID %>" />
        <div class="debugWindow">
            <div class="debug">Form Mode : {{mdata.formmode}}</div>
            <div class="debug">Display Mode : {{mdata.displaymode}}</div>
            <div class="debug">Submission ID : <%= SubmissionID %></div>
            <hr />
            <div class="debug">Session Abstract ID : <%= AbstractID %></div>
            <div class="debug">Session Evaluation ID : <%= EvaluationID %></div>
            <div class="debug">Session SubmissionType ID : <%= SessionSubmissionTypeId %></div>
            <div class="debug">Evaluated SubmissionType ID : <%= SubmissionTypeId %></div>
        </div>

     <div class="sixteen columns">
         <div>
             <span class="titles">Application ID :</span>
             <span class="titlevals"><%= applicationID %></span>

             <span class="titles">Project Title :</span>
             <span class="titlevals"><%= projectTitle %></span>
         </div>
     
     </div>

    <!-- <div class="sixteen columns">
         <div>
             <span class="titles">Last Name :</span>
             <span class="titlevals"><%= lastName %></span>
         </div>  
     </div>-->

       <div class="sixteen columns">
         <div>
             <span class="titles">User ID :</span>
             <span class="titlevals"><%= userName %></span>
         </div>  
     </div>

       <div class="sixteen columns">
         <div ng-show="mode.indexOf('Consensus') != -1">Users Unable to Code : <%= unableCoders %></div>
         <div ng-show="mode.indexOf('Comparison') != -1">Teams Unable to Code : <%= unableCoders %></div>
            <div><input type="checkbox" name="unabletocode" id="unabletocode" ng-model="mdata.unabletocode"  ng-disabled="mdata.displaymode=='View'" /><label>Unable to Code</label></div>
            <div>
            <div ng-show="mdata.unabletocode && mdata.displaymode!='View'">
                <input type="text" id="superusername" name="superusername" ng-model="mdata.superusername"  placeholder="supervisor username"/>
                <input type="password" id="superpassword" name="superpassword" ng-model="mdata.superpassword"  placeholder="supervisor password"/>
            </div>
            
            </div>
        </div>
    


    <div class="sixteen columns"> 

        <table class="bordered zebra-striped" id="study-focus" class="study-focus">
            <caption>A. Study Focus</caption>
            <thead>
                    <tr>
                            <th scope="col" class="question"></th>
                            <th scope="col" class="answer">A.1 Rationale</th>
                            <th scope="col" class="answer">A.2 Exposure</th>
                            <th scope="col" class="answer">A.3 Outcome</th>
                    </tr>
            </thead>
            <tbody>
            
                <% =studyFocusQuestions %>

            </tbody>
        </table>

        <table class="bordered zebra-striped" id="entities-studied">
            <caption>B. Entities Studied</caption>
            <thead>

            </thead>
            <tbody>

                <% = entitiesStudiedQuestions %>
                                              
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="study-setting">
            <caption>C. Study Setting</caption>
            <thead>

            </thead>
            <tbody>
                <% = studySettingsQuestions %>
                                     
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="population-focus">
            <caption>D. Population Focus</caption>
            <thead>

            </thead>
            <tbody>
                   <% = populationFocusQuestions %>
                   
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="study-design-purpose">
            <caption>E. Study Design/Purpose</caption>
            <thead>

            </thead>
            <tbody>
                  <% = studyDesignPurposeQuestions %>                               
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="prevention-research-category">
            <caption>F. Prevention Research Category</caption>
            <thead>

            </thead>
            <tbody>
                  <% = preventionCategoryQuestions %>         
            </tbody>
        </table>





        </div>

    <div class="sixteen columns" ng-show="mdata.displaymode=='Insert'"> 
        <div class="commentsHeader">Comments</div>
        <textarea name="comments" id="comments" ng-model="mdata.comments"></textarea>
    </div>
     <div class="sixteen columns" ng-show="mdata.displaymode=='View'"> 
         <div class="commentsHeader">Comments</div>
         <div id="commentsBox">{{ mdata.comments }}<%= Comments %></div>
     </div>

    </div>
</div>
</div>

    <!-- End Document
================================================== -->

<!-- JS
================================================== -->
<script src="../scripts/jquery.js"></script>
<script src="../scripts/main.js"></script>
<script src="../scripts/icheck.js"></script>
<script src="../scripts/alertify.js"></script>
<script src="../scripts/angular/angular.min.js"></script>

<script src="../scripts/modules/module.js"></script>
<script src="../scripts/controllers/controller.js"></script>
<script src="../scripts/directives/directive.js"></script>
<script src="../scripts/app.js"></script>
<!-- <script src="js/controllers/controller.js"></script> -->


    <!--[if lt IE 9]>
        <script src="js/html5.js"></script>
    <![endif]-->

