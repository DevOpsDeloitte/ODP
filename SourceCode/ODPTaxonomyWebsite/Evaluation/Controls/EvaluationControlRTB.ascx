<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EvaluationControlRTB.ascx.cs" Inherits="ODPTaxonomyWebsite.Evaluation.Controls.EvaluationControlRTB" %>


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

<div class="container" id="tax-form" ng-controller="ODPFormCtrlRT">
    <div>
    <nav class="cbp-spmenu cbp-spmenu-vertical cbp-spmenu-right comments" id="cbp-spmenu-s2" style="height: 253px;">
        <div class="comments-close-group">
            <a href="" id="showRightPushed" class="close-menu active">Close Comments</a>
            <a href="" class="expand-comments">Expand</a>
        </div>


<%--        <ul class="tabs-menu" ng-show="showComments() && showIQSCoders() && showODPCoders()">
            <li class="" ng-class="{ 'current' : showCoderDefault() }"><a href="#IQS">IQS Coders</a></li>
            <li class="" ng-class="{ 'current' : showODPDefault() }"><a href="#ODP">ODP Coders</a></li>
        </ul>--%>

        <div class="comment-box">
            <div class="" ng-show="mdata.displaymode=='Insertx'">
                    <div class="commentsHeader">Coding Comments</div>
                    <textarea name="comments" id="comments" ng-model="mdata.comments" disabled="disabled"></textarea>
                </div>
            <div class="comment-entry" ng-show="mdata.displaymode=='View' || mdata.displaymode=='Insert'">
                    <div class="commentsHeader">Coding Comments</div>
                    <%--<div id="commentsBox">{{ mdata.comments }}<%= Comments %></div>--%>
                   <div id="commentsBox" ng-bind-html="mdata.comments | newline"></div>
            </div>
        </div>

        <ul class="tabs-menu" ng-show="showComments() && showIQSCoders() && showODPCoders() && mdata.formmode != 'ODP Staff Member Consensus'">
            <li class="" ng-class="{ 'current' : showCoderDefault() }"><a href="#IQS">IQS Coders</a></li>
            <li class="" ng-class="{ 'current' : showODPDefault() }"><a href="#ODP">ODP Coders</a></li>
        </ul>


        <div class="tab" ng-show="showComments()">
            <div id="IQS" class="tab-content" style="" ng-class="{ 'current' : showCoderDefault() }" ng-show="showIQSCoders() && mdata.formmode != 'ODP Staff Member Consensus'">
                <%-- <textarea placeholder="Enter Comment here" style="height: 60px;"></textarea>--%>
                <div ng-show="mdata.formmode == 'ODP Staff Member Consensus' || mdata.formmode == 'ODP Staff Member Comparison'">
                    <strong>IQS Consensus Comments</strong>
                    <hr />
                    <div class="comment disabled">
                        <h5>{{mdata.CoderComments.IQConsensusUser.UserName}}</h5>
                        <p  ng-bind-html="mdata.CoderComments.IQConsensusUser.UserComment | newline"><%--{{mdata.CoderComments.IQConsensusUser.UserComment}}--%></p>
                    </div>
                </div>


                <strong>IQS Coders Comments</strong>
                <hr>

                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.IQCoders[0].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.IQCoders[0].UserComment | newline"><%--{{mdata.CoderComments.IQCoders[0].UserComment}}--%></p>
                </div>

                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.IQCoders[1].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.IQCoders[1].UserComment | newline"><%--{{mdata.CoderComments.IQCoders[1].UserComment}}--%></p>
                </div>

                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.IQCoders[2].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.IQCoders[2].UserComment | newline"><%--{{mdata.CoderComments.IQCoders[2].UserComment}}--%></p>
                </div>
            </div>
            <div id="ODP" class="tab-content" style="" ng-class="{ 'current' : showODPDefault() }" ng-show="showODPCoders()">
                <%--<textarea placeholder="Enter Comment here" style="height: 0px;"></textarea>--%>
                <div ng-show="mdata.formmode != 'ODP Staff Member Consensus'"">
                    <strong>ODP Consensus Comments</strong>
                    <hr />
                    <div class="comment disabled">
                        <h5>{{mdata.CoderComments.ODPConsensusUser.UserName}}</h5>
                        <p ng-bind-html="mdata.CoderComments.ODPConsensusUser.UserComment | newline"><%--{{mdata.CoderComments.ODPConsensusUser.UserComment}}--%></p>
                    </div>
                </div>


                <strong>ODP Coders Comments</strong>
                <hr>


                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.ODPCoders[0].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.ODPCoders[0].UserComment | newline"><%--{{mdata.CoderComments.ODPCoders[0].UserComment}}--%></p>
                </div>

                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.ODPCoders[1].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.ODPCoders[1].UserComment | newline"><%--{{mdata.CoderComments.ODPCoders[1].UserComment}}--%></p>
                </div>

                <div class="comment disabled">
                    <h5>{{mdata.CoderComments.ODPCoders[2].UserName}}</h5>
                    <p ng-bind-html="mdata.CoderComments.ODPCoders[2].UserComment | newline"><%--{{mdata.CoderComments.ODPCoders[2].UserComment}}--%></p>
                </div>
            </div>

        </div>

    </nav>
    </div>

    <div class="sixteen columns evalheader">
        <span class="subtitle">CHECK ALL THAT APPLY IN EACH COLUMN (TOPICS ARE NOT MUTUALLY EXCLUSIVE)</span>
        <span class="subtitle">See accompanying protocol for definitions and examples</span>
    </div>
    <div class="sixteen columns" ng-cloak>
        <h2><span ng-show="mdata.formmode.indexOf('Consensus') != -1 && mdata.formmode != undefined ">Consensus Watch</span><span ng-show="mdata.formmode.indexOf('Comparison') != -1  && mdata.formmode != undefined ">Comparison Watch</span> <span class="successmessages" ng-show="mdata.displaymode.indexOf('View') != -1 && mdata.displaymode != undefined">-- Completed</span><span ng-show="mdata.displaymode.indexOf('Insert') != -1 && mdata.displaymode != undefined"> -- In Progress</span> </h2>


    </div>

    <div class="subnav" ng-cloak>
        <ul>
            <li>
                <input class="button yes" type="button" id="printButton" value="Print Abstract" ng-click="printAbstract()" /></li>


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



            <div class="sixteen columns">
                <div>
                    <span class="titles">User ID :</span>
                    <span class="titlevals"><%= userName %></span>
                    <%--<br>
                    <span class="titles">Type :</span>
                    <span class="titlevals">Basic</span>--%>
                </div>

      <div class="sixteen columns tax-form-buttons">
            <div class="specs">
            <span class="buttonx">General Instructions<div class="icon open" id="ginst" ng-click="showDescription('generalinstructions')" ></div></span>
            <span class="buttonx">Background<div class="icon open" ng-click="showDescription('background')" ></div></span>
            </div>
      </div>  

                <div class="sixteen columns tax-form-buttons">

                    <div class="tax-form-buttons-box">
                        <div class="unableCodeBox button" id="unablecontain">
                            <div style="display: inline-block;">
                                <input type="checkbox" name="unabletocode" id="unabletocode" ng-model="mdata.unabletocode" ng-disabled=" 1==1 " /><label>Not Basic</label></div>
                            <!--<div class="icon open" ng-click="showDescription('unabletocode')"></div>-->

<%--                            <div id="unable-to-code" ng-show="mdata.unabletocode && mdata.displaymode!='View'">
                                <input type="text" id="superusername" name="superusername" ng-model="mdata.superusername" placeholder="supervisor username" />
                                <input type="password" id="superpassword" name="superpassword" ng-model="mdata.superpassword" placeholder="supervisor password" />
                            </div>--%>
                            <div ng-show="mdata.formmode.indexOf('Consensus') != -1 && mdata.formmode != undefined && mdata.unablecodersval != ''">Users Coded as Not Basic : {{mdata.unablecodersval}} </div>
                            <div ng-show="mdata.formmode.indexOf('Comparison') != -1 && mdata.formmode != undefined && mdata.unablecodersval != ''">Teams Coded as Not Basic : {{mdata.unablecodersval}} </div>

                        </div>
                    </div>

                   <%-- <div class="specs">
                        <span class="buttonx">General Instructions<div class="icon open" ng-click="showDescription('generalinstructions')"></div>
                        </span>
                        <span class="buttonx">Background<div class="icon open" ng-click="showDescription('background')"></div>
                        </span>
                    </div>--%>
                </div>



                <div class="sixteen columns main-buttons">

                    <table class="bordered zebra-striped" id="study-focus" class="study-focus">
                        <div class="captionTitle">A. Study Focus</div>
                        <div class="icon open" ng-click="showDescription('studyfocus-0')"></div>
                        <thead>
                            <tr>
                                <th scope="col" class="question">Topics<div class="icon open" ng-click="showDescription('topics-0')"></div>
                                </th>
                                <th scope="col" class="answer">A.4 Basic<div class="icon open" ng-click="showDescription('studyfocuscategory-4')"></div>
                                </th>
                               <%-- <th scope="col" class="answer">A.2 Exposure<div class="icon open" ng-click="showDescription('studyfocuscategory-2')"></div>
                                </th>
                                <th scope="col" class="answer">A.3 Outcome<div class="icon open" ng-click="showDescription('studyfocuscategory-3')"></div>
                                </th>--%>
                            </tr>
                        </thead>
                        <tbody>

                            <% =studyFocusQuestions %>
                        </tbody>
                    </table>

                    <table class="bordered zebra-striped" id="entities-studied">
                        <%--            <caption>B. Entities Studied</caption>--%>
                        <div class="captionTitle">B. Entities Studied</div>
                        <div class="icon open" ng-click="showDescription('entitiesstudied-0')"></div>
                        <thead>
                        </thead>
                        <tbody>

                            <% = entitiesStudiedQuestions %>
                        </tbody>
                    </table>

                    <table class="bordered zebra-striped" id="study-setting">
                        <%--            <caption>C. Study Setting</caption>--%>
                        <div id="ssetting" class="captionTitle category-toggle">C. Study Setting</div>
                        <div class="icon open" ng-click="showDescription('studysetting-0')"></div>
                        <thead>
                        </thead>
                        <tbody class="opener opener-ssetting hide-b">
                            <% = studySettingsQuestions %>
                        </tbody>
                    </table>

                    <table class="bordered zebra-striped" id="population-focus">
                        <%--            <caption>D. Population Focus</caption>--%>
                        <div id="pfocus" class="captionTitle category-toggle">D. Population Focus</div>
                        <div class="icon open" ng-click="showDescription('populationfocus-0')"></div>
                        <thead>
                        </thead>
                        <tbody class="opener opener-pfocus hide-b">
                            <% = populationFocusQuestions %>
                        </tbody>
                    </table>

                    <table class="bordered zebra-striped" id="study-design-purpose">
                        <%--            <caption>E. Study Design/Purpose</caption>--%>
                        <div id="sdp" class="captionTitle category-toggle">E. Study Design/Purpose</div>
                        <div class="icon open" ng-click="showDescription('studydesignpurpose-0')"></div>
                        <thead>
                        </thead>
                        <tbody class="opener opener-sdp hide-b">
                            <% = studyDesignPurposeQuestions %>
                        </tbody>
                    </table>

                    <table class="bordered zebra-striped" id="prevention-research-category">
                        <%--            <caption>F. Prevention Research Category</caption>--%>
                        <div id="prc" class="captionTitle category-toggle">F. Prevention Research Category</div>
                        <div class="icon open" ng-click="showDescription('preventioncategory-0')"></div>
                        <thead>
                        </thead>
                        <tbody class="opener opener-prc hide-b">
                            <% = preventionCategoryQuestions %>
                        </tbody>
                    </table>





                </div>

<%--                <div class="sixteen columns" ng-show="mdata.displaymode=='Insert'">
                    <div class="commentsHeader">Comments</div>
                    <textarea name="comments" id="comments" ng-model="mdata.comments" disabled="disabled"></textarea>
                </div>
                <div class="sixteen columns" ng-show="mdata.displaymode=='View'">
                    <div class="commentsHeader">Comments</div>
                    <div id="commentsBox">{{ mdata.comments }}<%= Comments %></div>
                </div>--%>

            </div>
        </div>
    </div>

    <!-- End Document
================================================== -->

    <!-- JS
================================================== -->
<%--    <script src="../scripts/jquery.js"></script>--%>
    <script src="../scripts/jquery.ns-autogrow.min.js"></script>
    <script src="../scripts/main.js"></script>
    <script src="../scripts/icheck.js"></script>
    <script src="../scripts/alertify.js"></script>
    <script src="../scripts/angular/angular-latest.min.js"></script>
    <script src="../scripts/angular/angular-sanitize.min.js"></script>
    <script src="../scripts/angular/firebase.js"></script>
    <script src="../scripts/angular/angularfire.min.js"></script>

    <%--
<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.4.9/angular.min.js"></script>
<script src="https://cdn.firebase.com/js/client/2.4.0/firebase.js"></script>
<script src="https://cdn.firebase.com/libs/angularfire/1.1.3/angularfire.min.js"></script>--%>


    <script src="../scripts/modules/module.js"></script>
    <script src="../scripts/controllers/controllerRTB.js"></script>
    <script src="../scripts/directives/directiveRTB.js"></script>
    <script src="../scripts/app.js"></script>
    <script src="../scripts/commentsRT.js"></script>
    <!-- <script src="js/controllers/controller.js"></script> -->


    <!--[if lt IE 9]>
        <script src="js/html5.js"></script>
    <![endif]-->
