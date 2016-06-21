﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs"
    Inherits="ODPTaxonomyWebsite.Evaluation.AbstractListViews.ODPSupervisorView_Default" %>
<%@ Register TagPrefix="odp" Namespace="ODPTaxonomyDAL_JY" Assembly="ODPTaxonomyDAL_JY" %>

<div class="sixteen columns" id="functionsbox"> 

    <div id="pagetitlebox">
    <h2>View Abstract List</h2>
            <h3><span>Title</span></h3>
    </div>

    <div id="filtersContainer">

        <div class="four columns filters interface">
            <label for="filterlist">Filters List</label>
            <select name="filterlist" id="filterlist" style="width:240px;"><option selected="selected" value="odpcompleted">ODP Completed (129)</option><option value="all">All Abstracts (3494)</option><option value="review">In Review List (0)</option><option value="reviewuncoded">In Review List Uncoded (0)</option><option value="codercompleted">Coder Completed (3079)</option><option value="activeabstracts">Active Abstracts (0)</option><option value="odpcompletedwonotes">ODP Completed without notes (1)</option><option value="closed">Closed (0)</option><option value="exported">Exported (285)</option><option value="reportexclude">Report Exclude List (9)</option></select>
        </div>

        <div class="three columns subactions interface" style="display: block;">
            <label for="actionlistlist">Actions List</label>
            <select name="actionlist" id="actionlist"><option selected="selected" value="selectaction">Select Action</option><option value="addreview">Add to Review List</option><option value="closeabstract">Close Abstracts</option><option value="addreportexclude">Add Report Exclude</option></select>
        </div>

        <div id="basicOnly" class="two columns" style="display: inline;">
            <input type="checkbox" id="cbBasicOnly"><label style="font-weight: bold">Basic Only</label>
        </div>

    </div>

    <%--<div id="filtersContainer">--%>

        <%--<div class="four columns filters interface">--%>
            <%--<label for="filterlist">Filters List</label>--%>
            <%--<select name="filterlist"  id="filterlist" style="width:240px;">--%>
                    <%--<option selected="selected" value="">Default View</option>--%>
                    <%--<option value="open">Open Abstract</option>--%>
                    <%--<option value="review">In Review List</option>--%>
                    <%--<option value="uncoded">In Review List - Uncoded Only</option>--%>
            <%--</select>--%>
        <%--</div>--%>

        <%--<div class="three columns subactions interface">--%>
            <%--<label for="actionlistlist">Actions List</label>--%>
            <%--<select name="actionlist"  id="actionlist">--%>
                    <%--<option selected="selected" value="addtoreview">Add to Review List</option>--%>
                    <%--<option value="closeabstracts">Close Abstracts</option>--%>
                    <%--<option value="reopenabstracts">Reopen Abstracts</option>--%>
                    <%--<option value="exportabstracts">Export Abstracts</option>--%>
            <%--</select>--%>
        <%--</div>--%>

        <%--<div class="two columns subactions interface">--%>
            <%--<input type="button" name="subButton" id="subButton" value="Submit" class="review button" />--%>
        <%--</div>--%>

        <%--<div class="three columns downloads interface">--%>
            <%--<!--<input type="checkbox"><label>label</label>-->--%>
            <%--<div id="downloadLinkBox">--%>
               <%--<a href="">Download Excel Report</a>--%>
            <%--</div>--%>
            <%--<div id="downloadProgressBox">--%>
                <%--<div id="downloadSpin"></div><div id="downloadText">Download Link being generated...</div>--%>
            <%--</div>--%>

            <%--<div id="generalProgressBox"><span>processing...</span></div>--%>
        <%--</div>--%>
    <%--</div>--%>

    <div class="five columns hidden" id="selectionsBox">
        <span id="recordCount">0</span><span> Records selected</span>
    </div>

    <%--<div id="basicOnly" class="two columns">--%>
        <%--<input type="checkbox" id="cbBasicOnly"><label style="font-weight: bold">Basic Only</label>--%>
    <%--</div>--%>


    <div id="informationalSection" style="display: inline-flex; position: absolute; top: 115px; left: 50%;" class="three columns downloads interface">
        <div id="downloadLinkBox" style="display: none;">
           <a href="">Download Excel Report</a>
        </div>

        <div id="downloadProgressBox">
            <div id="downloadSpin">
                <div class="spinner" role="progressbar" style="position: absolute; width: 0px; z-index: 2000000000; left: 0px; top: 0px;">
                    <div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-0-9 1s linear infinite;">
                        <div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(0deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div>
                    </div>

                    <div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-1-9 1s linear infinite;">
                        <div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(40deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div>
                    </div>
                    <div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-2-9 1s linear infinite;">
                        <div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(80deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div>
                    </div>
                    <div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-3-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(120deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div><div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-4-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(160deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div><div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-5-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(200deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div><div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-6-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(240deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div><div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-7-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(280deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div><div style="position: absolute; top: -1px; opacity: 0.25; animation: opacity-30-25-8-9 1s linear infinite;"><div style="position: absolute; width: 11px; height: 3px; box-shadow: rgba(0, 0, 0, 0.0980392) 0px 0px 1px; transform-origin: left center 0px; transform: rotate(320deg) translate(3px, 0px); border-radius: 1px; background: rgb(0, 0, 0);"></div></div>
                </div>
            </div>

            <div id="downloadText">Download Link being generated...</div>
        </div>

        <div id="generalProgressBox"><span>processing...</span></div>
    </div>


    <div id="submitSection" style="z-index: 999; display: inline-flex; position: absolute; top: 115px; left: 50%;" class="">
        <div class="two columns subactions interface" style="display: block;">
            <input type="button" name="subButton" id="subButton" value="Submit" class="review button no">
        </div>
    </div>

    <div class="progressBar">
        <div class="progressText">
             <span>Loading Records</span>
        </div>
        <div class="meter animate">
            <span style="width: 100%"><span></span></span>
        </div>
    </div>

    <!--
    <div class="sixteen columns" id="titlesbox">

    </div>
    -->

    <div id="tableContainer">
        <table id="DTable" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>

                    <th class="col_select"><input type="checkbox" name="selectAllBox" id="selectAllBox" value="selectall" class="cboxes" /><label for="selectAllBox"></label></th>
                    <th class="col_openclose"><input type="checkbox" name="allBox" id="allBox" value="expandall" class="cboxes" style="display: none;" /><label for="allBox"></label></th>
                    <th class="col_abstractid">Abstract ID</th>
                    <th class="col_applicationid">Application ID</th>
                    <th class="col_statusdate">Status Date</th>
                    <th class="col_piname">PI Name</th>
                    <th class="col_title">Title</th>
                    <th class="col_flags">Flags</th>
                    <th class="col_kappa">A1</th>
                    <th class="col_kappa">A2</th>
                    <th class="col_kappa">A3</th>
                    <th class="col_kappa">A4</th>
                    <th class="col_kappa">B</th>
                    <th class="col_kappa">C</th>
                    <th class="col_kappa">D</th>
                    <th class="col_kappa">E</th>
                    <th class="col_kappa">F</th>
                    <th class="col_exportdate">Exported Date</th>

                </tr>
            </thead>
        </table>
    </div>

</div>