<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ODPSupervisorView_Default.ascx.cs"
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
                <select name="filterlist"  id="filterlist" style="width:240px;">
                        <option selected="selected" value="">Default View</option>
                        <option value="open">Open Abstract</option>
                        <option value="review">In Review List</option>
                        <option value="uncoded">In Review List - Uncoded Only</option>
                </select>
            </div>

            <div class="three columns subactions interface">
                <label for="actionlistlist">Actions List</label>
                <select name="actionlist"  id="actionlist">
                        <option selected="selected" value="addtoreview">Add to Review List</option>
                        <option value="closeabstracts">Close Abstracts</option>
                        <option value="reopenabstracts">Reopen Abstracts</option>
                        <option value="exportabstracts">Export Abstracts</option>
                </select>
            </div>
            <div class="two columns subactions interface">
                <input type="button" name="subButton" id="subButton" value="Submit" class="review button" />
            </div>

            <div class="three columns downloads interface">
                <div id="downloadLinkBox">
                   <a href="">Download Excel Report</a>
                </div>
                <div id="downloadProgressBox">
                    <div id="downloadSpin"></div><div id="downloadText">Download Link being generated...</div>
                </div>

                <div id="generalProgressBox"><span>processing...</span></div>
            </div>
    </div>

    <div class="five columns hidden" id="selectionsBox">
        <span id="recordCount">0</span><span> Records selected</span>
    </div>




    <script type="text/javascript">

       // window.config.role = "ODPSupervisor";

    </script>


    <div class="progressBar">
        <div class="progressText">
             <span>Loading Records</span>
        </div>
        <div class="meter animate">
            <span style="width: 100%"><span></span></span>
        </div>
    </div>

    <div class="sixteen columns" id="titlesbox">

    </div>

    <div id="tableContainer">
        <table id="DTable" class="display" cellspacing="0" width="100%">
            <thead>
                <tr>

                    <th class="col_select"><input type="checkbox" name="selectallBox" id="selectallBox" value="selectall" class="cboxes" /><label for="selectallBox"></label></th>
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