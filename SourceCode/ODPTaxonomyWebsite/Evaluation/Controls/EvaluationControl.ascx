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
<div>
<div class="container" ng-controller="ODPFormCtrl">

    <div class="sixteen columns header"> 
        <h1 class="center">Prevention Taxonomy Form</h1>
        <span class="subtitle">CHECK ALL THAT APPLY IN EACH COLUMN (TOPICS ARE NOT MUTUALLY EXCLUSIVE)</span>
        <span class="subtitle">See accompanying protocol for definitions and examples</span>
    </div>

    <div class="nav">
        <ul>
            <li><a href="#study-focus">Study Focus</a></li>
            <li><a href="#entities-studied">Entities Studied</a></li>
            <li><a href="#study-setting">Study Setting</a></li>
            <li><a href="#population-focus">Population focus</a></li>
            <li><a href="#study-design-purpose">Study Design/Purpose</a></li>
            <li><a href="#prevention-research-category">Prevention Research Category</a></li>
            <!--<li><a class="button" href="#" id="confirmX" ng-click="processForm()" ng-disabled="{{1 == 1}}">Save</a></li>-->
            <li ng-show="showResetButton"><input class="button" type="button" id="resetButton" value="Reset" ng-click="resetForm()" /></li>
            <li ng-show="showConsensusButton"><input class="button" type="button" id="consensusButton" value="Start Consensus" ng-click="startConsensus()" /></li>
            <li ng-show="showSaveButton"><input class="button" type="button" id="saveButton" value="Save" ng-click="processForm()" ng-disabled="disallowSave" /></li>
            <li><input type="checkbox" name="unabletocode" id="unabletocode" ng-model="mdata.unabletocode"  ng-disabled="mdata.displaymode=='View'"><label>Unable to Code</label></li>
        </ul>
    </div>
    <div id="odpforms" name="x" >
    <div class="sixteen columns"> 
            <div>
<%--              <label>Name:</label>
              <input type="text" name="testid" ng-model="mdata.note" placeholder="Enter a note here" />
              <hr>
              <h1> {{mdata.note}}</h1>
              <h1> {{mdata.count}}</h1>
          
                 <input type="checkbox" ng-model="mdata.studyfocus_1_1">
            </div>
            <div class="field">
                <strong>Notes :</strong>
                <div click-to-edit="mdata.note"></div>
            </div>
         <tt>value1 = {{value1}}</tt><br/>--%>


        <div>
            <h2>{{postmessages}}</h2>
        </div>
    </div>
        <input type="hidden" name="submissionid" value="<%= SubmissionID %>" />
        <input type="hidden" name="mode" id="mode" value="<%= FormMode %>" ng-model="mdata.formmode" />
        <input type="hidden" name="displaymode" id="displaymode" value="<%= DisplayMode %>" ng-model="mdata.displaymode" />  
        <input type="hidden" name="userid" id="userid" value="<%= UserId %>" ng-model="mdata.userid" />
        <input type="hidden" name="submissiontypeid" id="submissiontypeid" value="<%= SubmissionTypeId %>" ng-model="mdata.submissiontypeid" />
        <input type="hidden" name="abstractid" id="abstractid" value="<%= AbstractID %>" ng-model="mdata.abstractid" />
        <input type="hidden" name="showc" id="showc" value="<%= showConsensusButton %>" ng-model="mdata.showconsensusbutton" />
        <!-- not need just a placholder for now -->
        <input type="hidden" name="evaluationid" value="<%= EvaluationID %>" />
        <div class="debugWindow">
            <div class="debug">Form Mode : {{mdata.formmode}}</div>
            <div class="debug">Display Mode : {{mdata.displaymode}}</div>
            <div class="debug">Submission ID : <%= SubmissionID %></div>
            <hr />
            <div class="debug">Session Abstract ID : <%= AbstractID %></div>
            <div class="debug">Session Evaluation ID : <%= EvaluationID %></div>
            <div class="debug">Session SubmissionType ID : <%= SubmissionTypeId %></div>
        </div>

     <div class="sixteen columns">
         <div>
             <span class="titles">Application ID :</span>
             <span class="titlevals"><%= applicationID %></span>

             <span class="titles">Project Title :</span>
             <span class="titlevals"><%= projectTitle %></span>
         </div>
     
     </div>

     <div class="sixteen columns">
         <div>
             <span class="titles">Last Name :</span>
             <span class="titlevals"><%= lastName %></span>

             
         </div>
     
     </div>
    
     <div class="sixteen columns" ng-show="mdata.displaymode=='Insert'"> 
         <div>Comments</div>
         <textarea name="comments" id="comments" ng-model="mdata.comments"></textarea>
     </div>
     <div class="sixteen columns" ng-show="mdata.displaymode=='View'"> 
         <div>Comments</div>
         <div id="commentsBox">{{ mdata.comments }}<%= Comments %></div>
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

