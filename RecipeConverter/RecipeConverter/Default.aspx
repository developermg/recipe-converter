<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RecipeConverterApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container">
        <div class="row" id="mainDiv">
            <div class="col-lg-6">
                <div class="well bs-component form-horizontal">
                    
                        <fieldset>
                            <legend>Original Recipe</legend>
                            <div class="form-group">
                                <label for="TitleText" class="col-lg-3 control-label">Recipe Title</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="TitleText" runat="server" CssClass="form-control" ToolTip="Enter the recipe title"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="OriginalServingsText" class="col-lg-3 control-label">Original Servings</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="OriginalServingsText" CssClass="form-control" runat="server" ToolTip="Enter the original serving size" TextMode="Number"></asp:TextBox>
                                    <asp:RangeValidator ControlToValidate="OriginalServingsText" runat="server" Type="Integer"
                                        MinimumValue="1" MaximumValue="1000" ErrorMessage="Original servings must be between 1 and 1000" display="dynamic" ForeColor="#d9534f"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ControlToValidate="OriginalServingsText" runat="server" ErrorMessage="Original servings (numeric value) required" display="dynamic" ForeColor="#d9534f"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="DesiredServingsText" class="col-lg-3 control-label">Desired Servings</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="DesiredServingsText" CssClass="form-control" runat="server" ToolTip="Enter the desired serving size" TextMode="Number"></asp:TextBox>
                                    <asp:RangeValidator ControlToValidate="DesiredServingsText" runat="server" Type="Integer"
                                        MinimumValue="1" MaximumValue="1000" ErrorMessage="Desired servings must be between 1 and 1000" display="dynamic" ForeColor="#d9534f"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ControlToValidate="DesiredServingsText" runat="server" ErrorMessage="Desired servings (numeric value) required" display="dynamic" ForeColor="#d9534f"/>

                                </div>
                            </div>
                            <div class="form-group">
                                <label for="IngredientsText" class="col-lg-3 control-label">Ingredients</label>
                                <div class="col-lg-9">
                                    <asp:TextBox ID="IngredientsText" CssClass="form-control" runat="server" ToolTip="Paste or type the ingredients list here " TextMode="MultiLine" Rows="6"></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="IngredientsText" runat="server" ErrorMessage="Ingredients list required" display="dynamic" ForeColor="#d9534f"></asp:RequiredFieldValidator>

                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-lg-9 col-lg-offset-3" id="buttons">
                                    <asp:Button ID="ResetButton" runat="server" Text="Clear" OnClick="ResetButton_Click" CausesValidation="false" class="col-lg-offset-3 col-lg-3 btn btn-default button-row" />
                                    <asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="SubmitButton_Click" class="col-lg-3 btn btn-primary button-row" />
                                </div>
                            </div>
                        </fieldset>
                  
                </div>
            </div>
            <div class="col-md-4">
                <h2>Resized Recipe</h2>
                <asp:Label ID="RecipeTitleLabel" runat="server" Text="" Font-Bold="True"></asp:Label>
                <br />
                <asp:Label ID="ResizedVersionLabel" runat="server" Text="Resized recipe will go here."></asp:Label>

            </div>
        </div>
    </div>
</asp:Content>
