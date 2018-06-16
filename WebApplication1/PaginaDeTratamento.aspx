<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PaginaDeTratamento.aspx.cs" Inherits="WebApplication1.PaginaDeTratamento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Ops! Ocorreu algo que não esperávamos. Poderia reportar esta situação para o administrador?
    <br/><br/>
    <asp:Label ID="LabelErro" runat="server" Text=""></asp:Label><br />
    <br />
    <asp:Label ID="LabelnnerException" runat="server" Text=""></asp:Label>
    <br />
    <br />
</asp:Content>
