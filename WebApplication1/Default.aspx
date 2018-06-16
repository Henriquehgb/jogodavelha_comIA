<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="PanelJogo" runat="server" Visible="False">
        <div style="border: darkslateblue; border-width: medium">
            <asp:Label runat="server" Text="Placar:" ID="LabelPlacar" />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" Text="Jogador 1: " ID="LabelJogador1" />
            <asp:Label runat="server" Text="" ID="LabelPlacarJogador1" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" Text="Jogador 2: " ID="LabelJogador2" />
            <asp:Label runat="server" Text="" ID="LabelPlacarJogador2" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label runat="server" Text="Velha: " ID="LabelVelha" />
            <asp:Label runat="server" Text="" ID="LabelPlacarVelha" />
        </div>
        <div style="padding: 5px 5px">
            <div>
                <br />
                <asp:Label runat="server" ID="labelJogadorDaVez"></asp:Label>
                <br />
                <br />
                <asp:Label runat="server" Text="Mensagem: " /><asp:Label runat="server" ID="labelMensagemPrincipal" />
                <br />
                <br />
            </div>
        </div>
        <%--<div class="Coluna50">--%>
        <asp:GridView ID="GridViewTabuleiro" runat="server" ShowHeader="False" OnRowDataBound="GridViewTabuleiro_OnRowDataBound"
            OnRowCommand="GridViewTabuleiro_RowCommand" Font-Size="XX-Large" Font-Bold="True"
            Font-Overline="False">
            <Columns>
                <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False" />
            </Columns>
            <RowStyle Height="60px" />
        </asp:GridView>
        <br />
        <asp:Button ID="ButtonReiniciar" runat="server" Text="Jogar Novamente" OnClick="ButtonReiniciar_OnClick" />
        &nbsp;
        <asp:Button ID="ButtonTerminarJogo" runat="server" Text="Encerrar Jogo" OnClick="ButtonTerminarJogo_OnClick" />
    </asp:Panel>
    <asp:Panel ID="PanelEscolhaDeJogo" runat="server">
        <br />
        <asp:Label runat="server" Text="Bem Vindo ao Jogo da Velha!"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Escolha as opções de jogo aqui em baixo, clique em 'Iniciar Jogo' e divirta-se."></asp:Label>
        <br />
        <br />
        <asp:RadioButtonList ID="RadioButtonListTipoDeOponente" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="RadioButtonListTipoDeOponente_OnSelectedIndexChanged">
            <asp:ListItem Selected="True" Value="0">Jogar contra um amigo ao seu lado.</asp:ListItem>
            <asp:ListItem Value="1">Jogar contra o computador. (O computador aprenderá conforme vocês forem jogando)</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <asp:Label ID="LabelNomeDoJogador1" runat="server" Text="Digite o seu Nome Aqui"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxNomeDoJogador1" runat="server" Width="350px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorNomeObrigatorio" runat="server"
            ErrorMessage="Por Favor Informe o Nome" ControlToValidate="TextBoxNomeDoJogador1">É Obrigatório Informar o Nome</asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Panel ID="PanelDadosDoPanelJogador2" runat="server">
            <asp:Label ID="Label2" runat="server" Text="Digite o Nome do Seu Amigo Aqui"></asp:Label>
            <br />
            <asp:TextBox ID="TextBoxNomeDoAmigo" runat="server" Width="350px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorNomeDoAmigoObrigatorio" runat="server"
                ErrorMessage="Por Favor Informe o Nome do Amigo" ControlToValidate="TextBoxNomeDoAmigo">É Obrigatório Informar o Nome do Amigo</asp:RequiredFieldValidator>
        </asp:Panel>
        <br />
        <asp:Label ID="LabelEscala" runat="server" Text="Escala do Tabuleiro (3 até 6)"></asp:Label><br />
        <asp:TextBox ID="TextBoxEscala" Text="3" runat="server" Width="40px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEscalaObrigatoria" runat="server"
            ErrorMessage="Por Favor Informe a Escala" ControlToValidate="TextBoxEscala">É Obrigatório Informar a Escala</asp:RequiredFieldValidator>
        &nbsp;&nbsp;
        <asp:RangeValidator ID="RangeValidatorRangeDaEscala" runat="server" ErrorMessage="RangeValidator"
            ControlToValidate="TextBoxEscala" MaximumValue="6" MinimumValue="3">A Escala Deve ser de 3 até 6</asp:RangeValidator>
        <br />
        <br />
        <asp:Button runat="server" ID="IniciarJogo" Text="Iniciar Jogo" OnClick="IniciarJogo_OnClick" />
    </asp:Panel>
</asp:Content>
