<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ODPTaxonomyTrainingAdminWebsite._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="sixteen columns sub-title"> 
    <h2>HOMEPAGE</h2>
</div>            
<div class="sixteen columns"> 
    <asp:Label runat="server" CssClass="errorMessage" ID="lbl_Error" class="panel" Visible="false"></asp:Label>
    <asp:Label runat="server" CssClass="regularMessage" ID="lbl_message" class="panel" Visible="false"></asp:Label>   
    
    <div>
        <p>Below are instructions for populating ODP abstract coding from Anthony's instance (#2) across to other instances and generating Kappas for those instances.</p>
        <ol>
            <li>From the dropdown, choose the active instance. This is the instance that you want to replicate the ODP answers into.</li>
            <li>Press the button "Populate ODP Selections."</li>
            <li>After receiving the success message, you can then press the "Push Trainee Data" button. This moves trainee data to SAS and begins the Kappa calculation process. </li>
            <li>After 15 minutes, press the "Pull Trainee Kappa" button to populate Kappa data from SAS into the Active instance. </li>
            <li>To see Kappas for an individual, log in to that instance and as an ODP Supervisor, view the Abstract List. </li>
        </ol>

        <p>To generate Kappas for Anthony's data:</p>
        <ol>
            <li>Choose Instance 2 as the active instance.</li>
            <li>Press the "Push Trainee Data" button. This moves trainee data to SAS and begins the Kappa calculation process. </li>
            <li>After 15 minutes, press the "Pull Trainee Kappa" button to populate Kappa data from SAS into the instance. </li>
            <li>Log in to Anthony's instance, and as an ODP Supervisor, view the Abstract List. </li>
        </ol>
    </div>

    <asp:Panel runat="server" ID="pnl_training" CssClass="panel">
        <span class="subtitle center">Training</span>

        <div class="center">
            <asp:DropDownList ID="ddl_instances" runat="server" Width="250px" />
            <asp:Button class="button" runat="server" ID="btn_populate_odp" Text="Populate ODP Selections" onclick="btn_populate_odp_Click" />
            <asp:Button class="button" runat="server" ID="btn_push" Text="Push Trainee Data" onclick="btn_push_Click" />
            <asp:Button class="button" runat="server" ID="btn_pull" Text="Pull Trainee KAPPA" onclick="btn_pull_Click"/>
        </div>
    </asp:Panel>
    </div>
</asp:Content>
