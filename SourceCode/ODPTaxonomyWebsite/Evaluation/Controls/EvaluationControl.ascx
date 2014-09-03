<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluationControl.ascx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Controls.EvaluationControl" %>


	<!-- CSS
  ================================================== -->
  <link rel="stylesheet" href="../styles/alertify.css">    
  <link rel="stylesheet" href="../styles/line/tax.css">


	<!-- Favicons
	================================================== -->
	<link rel="shortcut icon" href="../images/favicon.ico">
	<link rel="apple-touch-icon" href="../images/apple-touch-icon.png">
	<link rel="apple-touch-icon" sizes="72x72" href="../images/apple-touch-icon-72x72.png">
	<link rel="apple-touch-icon" sizes="114x114" href="../images/apple-touch-icon-114x114.png">
<script type="text/javascript">
   // $scope.mdata.formmode = "<%= FormMode %>"
</script>

<div class="container" id="tax-form" ng-controller="ODPFormCtrl">

    <div class="sixteen columns evalheader"> 
        <span class="subtitle">CHECK ALL THAT APPLY IN EACH COLUMN (TOPICS ARE NOT MUTUALLY EXCLUSIVE)</span>
        <span class="subtitle">See accompanying protocol for definitions and examples</span>
    </div>
    <div class="sixteen columns" ng-cloak>
    <h2 ng-show="mdata.formmode.indexOf('Evaluation') != -1">Individual Coding</h2>
    <h2 ng-show="mdata.formmode.indexOf('Consensus') != -1">Consensus Coding</h2>
    <h2 ng-show="mdata.formmode.indexOf('Comparison') != -1">Comparison Coding</h2>
    </div>

    <div class="subnav" ng-cloak>
        <ul>
            <!--<li><a href="#study-focus">Study Focus</a></li>
            <li><a href="#entities-studied">Entities Studied</a></li>
            <li><a href="#study-setting">Study Setting</a></li>
            <li><a href="#population-focus">Population focus</a></li>
            <li><a href="#study-design-purpose">Study Design/Purpose</a></li>
            <li><a href="#prevention-research-category">Prevention Research Category</a></li>-->

            <!--<li><a class="button" href="#" id="confirmX" ng-click="processForm()" ng-disabled="{{1 == 1}}">Save</a></li>-->
            <li ng-show="showWatchConsensusButton"><input class="button yes" type="button" id="watchConsensus" value="Watch Consensus" ng-click="watchConsensus()" /></li>
            <li ng-show="!showWatchConsensusButton && mode.indexOf('Evaluation') != -1"><input class="button no" type="button" id="disabledwatchConsensusButton" value="Watch Consensus" /></li>
             <li ng-show="showWatchComparisonButton"><input class="button yes" type="button" id="watchComparison" value="Watch Comparison" ng-click="watchComparison()" /></li>
            <li ng-show="!showWatchComparisonButton && mode.indexOf('Evaluation') != -1"><input class="button no" type="button" id="disabledwatchComparisonButton" value="Watch Comparison" /></li>

            <li><input class="button yes" type="button" id="printButton" value="Print Abstract" ng-click="printAbstract()" /></li>
            <li ng-show="showResetButton"><input class="button yes" type="button" id="resetButton" value="Reset" ng-click="resetFormStart()" /></li>
            <li ng-show="showComparisonButton"><input class="button yes" type="button" id="comparisonButton" value="Start Comparison" ng-click="startComparison()" /></li>
            <li ng-show="!showComparisonButton && mode.indexOf('Comparison') != -1"><input class="button no" type="button" id="disabledcomparisonButton" value="Start Comparison" /></li>
            <li ng-show="showConsensusButton"><input class="button yes" type="button" id="consensusButton" value="Start Consensus" ng-click="startConsensus()" /></li>
            <li ng-show="!showConsensusButton && mode.indexOf('Consensus') != -1"><input class="button no" type="button" id="disabledconsensusButton" value="Start Consensus" /></li>
            <li ng-show="showSaveButton"><input class="button yes" type="button" id="saveButton" value="Submit" ng-click="processForm()" ng-disabled="disallowSave" /></li>
            <li ng-show="!showSaveButton"><input class="button no" type="button" id="disabledSaveButton" value="Submit"/></li>
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
            <h2 class="successmessages">{{postmessages}}</h2>
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

        <input type="hidden" name="consensusalreadystarted" id="consensusalreadystarted" value="<%= consensusAlreadyStarted %>" ng-model="mdata.consensusalreadystarted" />

        <input type="hidden" name="teamid" id="teamid" value="<%= teamID %>" ng-model="mdata.teamid" />

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
             <br />
             <span class="titles">Project Title :</span>
             <span class="titlevals"><%= projectTitle %></span>
             <br />
             <span class="titles">PI Project Leader :</span>
             <span class="titlevals"><%= piProjectLeader %></span>
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
     

       <div class="sixteen columns tax-form-buttons">
          
          <div class="tax-form-buttons-box">
            <div class="unableCodeBox button">              
              <div style="display: inline-block;"><input type="checkbox" name="unabletocode" id="unabletocode" ng-model="mdata.unabletocode"  ng-disabled="mdata.displaymode=='View'" /><label>Unable to Code</label></div><div class="icon open" ng-click="showDescription('unabletocode')" ></div>
                
              <div id="unable-to-code" ng-show="mdata.unabletocode && mdata.displaymode!='View'">
                    <input type="text" id="superusername" name="superusername" ng-model="mdata.superusername"  placeholder="supervisor username"/>
                    <input type="password" id="superpassword" name="superpassword" ng-model="mdata.superpassword"  placeholder="supervisor password"/>
              </div>
              <div ng-show="mode.indexOf('Consensus') != -1">Users Unable to Code : <%= unableCoders %></div>
              <div ng-show="mode.indexOf('Comparison') != -1">Teams Unable to Code : <%= unableCoders %></div>
            
            </div>
          </div>

            <div class="specs">
            <span class="buttonx">General Instructions<div class="icon open" id="ginst" ng-click="showDescription('generalinstructions')" ></div></span>
            <span class="buttonx">Background<div class="icon open" ng-click="showDescription('background')" ></div></span>
            </div>
        </div>
    


    <div class="sixteen columns"> 

        <table class="bordered zebra-striped" id="study-focus" class="study-focus">
            <div class="captionTitle">A. Study Focus</div>
            <div class="icon open" ng-click="showDescription('studyfocus-0')" ></div>
            <thead>
                    <tr>
                            <th scope="col" class="question">Topics<div class="icon open" ng-click="showDescription('topics-0')" ></div></th>
                            <th scope="col" class="answer">A.1 Rationale<div class="icon open" ng-click="showDescription('studyfocuscategory-1')" ></div></th>
                            <th scope="col" class="answer">A.2 Exposure<div class="icon open" ng-click="showDescription('studyfocuscategory-2')" ></div></th>
                            <th scope="col" class="answer">A.3 Outcome<div class="icon open" ng-click="showDescription('studyfocuscategory-3')" ></div></th>
                    </tr>
            </thead>
            <tbody>
            
                <% =studyFocusQuestions %>

            </tbody>
        </table>

        <table class="bordered zebra-striped" id="entities-studied">
<%--            <caption>B. Entities Studied</caption>--%>
            <div class="captionTitle">B. Entities Studied</div>
            <div class="icon open" ng-click="showDescription('entitiesstudied-0')" ></div>
            <thead>

            </thead>
            <tbody>

                <% = entitiesStudiedQuestions %>
                                              
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="study-setting">
<%--            <caption>C. Study Setting</caption>--%>
            <div class="captionTitle">C. Study Setting</div>
            <div class="icon open" ng-click="showDescription('studysetting-0')" ></div>
            <thead>

            </thead>
            <tbody>
                <% = studySettingsQuestions %>
                                     
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="population-focus">
<%--            <caption>D. Population Focus</caption>--%>
            <div class="captionTitle">D. Population Focus</div>
            <div class="icon open" ng-click="showDescription('populationfocus-0')" ></div>
            <thead>

            </thead>
            <tbody>
                   <% = populationFocusQuestions %>
                   
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="study-design-purpose">
<%--            <caption>E. Study Design/Purpose</caption>--%>
            <div class="captionTitle">E. Study Design/Purpose</div>
            <div class="icon open" ng-click="showDescription('studydesignpurpose-0')" ></div>
            <thead>

            </thead>
            <tbody>
                  <% = studyDesignPurposeQuestions %>                               
            </tbody>
        </table>

           <table class="bordered zebra-striped" id="prevention-research-category">
<%--            <caption>F. Prevention Research Category</caption>--%>
            <div class="captionTitle">F. Prevention Research Category</div>
            <div class="icon open" ng-click="showDescription('preventioncategory-0')" ></div>
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
<script src="../scripts/angular/angular-latest.min.js"></script>
<script src="../scripts/angular/firebase.js"></script>
<script src="../scripts/angular/angularfire.min.js"></script>
<!--<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.0-beta.18/angular.min.js"></script>-->
  <!-- Firebase -->
<!--<script src="https://cdn.firebase.com/js/client/1.0.18/firebase.js"></script>-->
<!-- AngularFire Library -->
<!--<script src="https://cdn.firebase.com/libs/angularfire/0.8.0/angularfire.min.js"></script>-->

<script src="../scripts/modules/module.js"></script>
<script src="../scripts/controllers/controller.js"></script>
<script src="../scripts/directives/directive.js"></script>
<script src="../scripts/app.js"></script>
<!-- <script src="js/controllers/controller.js"></script> -->


    <!--[if lt IE 9]>
        <script src="js/html5.js"></script>
    <![endif]-->

