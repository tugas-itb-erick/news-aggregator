<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="WebApplication2.WebForm1" %>
<%@ Import Namespace="System.ServiceModel.Syndication" %>
<%@ Import Namespace="algo" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/x-icon" href="emoji.png" />
    <title>Borborygmutty | Search</title>

    <!-- Bootstrap -->
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/modern-business.css" rel="stylesheet" />
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="container">
            <div class="navbar-brand">
                <img src="emoji.png" width="25" height="25" class="img-responsive" />
            </div>
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-ex1-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <!-- You'll want to use a responsive image option so this logo looks good on devices - I recommend using something like retina.js (do a quick Google search for it and you'll find it) -->
                <ul class="nav navbar-nav navbar-header">
                    <li id="home">
                        <a href="Index.html">Borborygmutty</a>
                    </li>
                    <li id="search" class="active">
                        <a href="Search.aspx"><span class="glyphicon glyphicon-search"></span></a>
                    </li>
                </ul>
            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse navbar-ex1-collapse">
                <ul class="nav navbar-nav navbar-right">
                    <li id="video">
                        <a href="Video.html"><img src="movie.png" width="20" height="20" /></a>
                    </li>
                    <li id="about">
                        <a href="About.html"><span class="glyphicon glyphicon-user"></span> About</a>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container -->
    </nav>

        <div class="form">
            <form action="Search.aspx" method="get">
            <div class="container center-block">
                <div class="row"><br /><br /></div>
                <div class="row">
                    <div class="col-sm-1">
                        <img src="logo.PNG" width="150" height="80" />
                    </div>
                    <div class="col-sm-1"></div>
                    <div class="col-sm-8 center-block text-center">
                        <div class="input-group">
                            <input class="form-control center-block" id="inputsm" name="keyword" type="text" placeholder="Type Text Here" />
                            <div class="input-group-btn">
                                <button class="btn btn-default" role="button" type="submit">
                                     <i class="glyphicon glyphicon-search"></i>
                                </button>

                            </div>
                            </div>
                        </div>
                    <div class="col-sm-3">

                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <label class="radio-inline"><input type="radio" name="optradio" value="BoyerMoore" />Boyer Moore &nbsp;&nbsp;</label>
                        <label class="radio-inline"><input type="radio" name="optradio" value="KMP" />KMP &nbsp;&nbsp;</label>
                        <label class="radio-inline"><input type="radio" name="optradio" value="Regex" />Regex</label>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <!-- /.row -->
                </div>
                </form>
            </div>
    <!-- /.section-colored -->

    <div class="content">
        <br /><br /><br /><br />
        <%
            string keyword = Request.QueryString["keyword"];
            string optradio = Request.QueryString["optradio"];

            List<Tuple<int, int>> result = new List<Tuple<int, int>>();

            if (keyword == "")
            {
                Response.Write("<div class='row'><p align='center'>Please input the keyword(s).</p></div>");
            }
            else
            {
                if (optradio == "BoyerMoore")
                {
                    if (Matcher.bmMatch(MainProgram.news.ToString(), keyword) == -1)
                    {
                        Response.Write("<div class='row'><p align='center'>Search Not Found.</p></div>");
                    }
                    else
                    {
                        Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h5><B><I>Boyer Moore Algorithm</I></B></div></div>");
                        result = MainProgram.news.GetSearchResultWithBM(keyword);
                        for (int i = 0; i < result.Count; i++)
                        {
                            Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h4><B>" + MainProgram.news.Get(result[i].Item1).GetTitle() + "</B></h4></div></div><br>");
                            Response.Write("<div class='row'><div class='col-md-1'></div><div class='col-md-5'><p align='justify'>" + MainProgram.news.Get(result[i].Item1).GetContentSummary(result[i].Item2) + "</p></div><div class='col-md-6'><img class='img-circle' width='100' height='100' src=" + MainProgram.news.Get(result[i].Item1).GetImageURL() + "></div></div>");
                            if (MainProgram.news.Get(result[i].Item1).getLocation() == "")
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a></blockquote></div></div>");
                            } else
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a><font size='2' color='white'>location</font><a href=" + MainProgram.news.Get(result[i].Item1).getGoogleMapsLink( MainProgram.news.Get(result[i].Item1).getLocation()) + " target='_blank'><img src='Map.png' height='40' width='40'></a></blockquote></div></div>");
                            }
                            Response.Write("<br>");
                        }
                    }
                }
                else if (optradio == "KMP")
                {
                    if (Matcher.kmpMatch(MainProgram.news.ToString(), keyword) == -1)
                    {
                        Response.Write("<div class='row'><p align='center'>Search Not Found.</p></div>");
                    }
                    else
                    {
                        Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h5><B><I>Knuth-Morris-Pratt Algorithm</I></B></div></div>");
                        result = MainProgram.news.GetSearchResultWithKMP(keyword);
                        for (int i = 0; i < result.Count; i++)
                        {
                            Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h4><B>" + MainProgram.news.Get(result[i].Item1).GetTitle() + "</B></h4></div></div><br>");
                            Response.Write("<div class='row'><div class='col-md-1'></div><div class='col-md-5'><p align='justify'>" + MainProgram.news.Get(result[i].Item1).GetContentSummary(result[i].Item2) + "</p></div><div class='col-md-6'><img class='img-circle' width='100' height='100' src=" + MainProgram.news.Get(result[i].Item1).GetImageURL() + "></div></div>");
                            if (MainProgram.news.Get(result[i].Item1).getLocation() == "")
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a></blockquote></div></div>");
                            } else
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a><font size='2' color='white'>location</font><a href=" + MainProgram.news.Get(result[i].Item1).getGoogleMapsLink( MainProgram.news.Get(result[i].Item1).getLocation()) + " target='_blank'><img src='Map.png' height='40' width='40'></a></blockquote></div></div>");
                            }
                            Response.Write("<br>");
                        }
                    }

                }
                else if (optradio == "Regex")
                {
                    if (Matcher.regexMatch(MainProgram.news.ToString(), keyword) == -1)
                    {
                        Response.Write("<div class='row'><p align='center'>Search Not Found.</p></div>");
                    }
                    else
                    {
                        Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h5><B><I>Regular Expression Algorithm</I></B></div></div>");
                        result = MainProgram.news.GetSearchResultWithRegex(keyword);
                        for (int i = 0; i < result.Count; i++)
                        {
                            Response.Write("<div class='row'><div class='col-sm-1'></div><div class='col-sm-11'><h4><B>" + MainProgram.news.Get(result[i].Item1).GetTitle() + "</B></h4></div></div><br>");
                            Response.Write("<div class='row'><div class='col-md-1'></div><div class='col-md-5'><p align='justify'>" + MainProgram.news.Get(result[i].Item1).GetContentSummary(result[i].Item2) + "</p></div><div class='col-md-6'><img class='img-circle' width='100' height='100' src=" + MainProgram.news.Get(result[i].Item1).GetImageURL() + "></div></div>");
                            if (MainProgram.news.Get(result[i].Item1).getLocation() == "")
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a></blockquote></div></div>");
                            } else
                            {
                                Response.Write("<br><div class='row'><div class='col-sm-1'></div><div class='col-sm-10'><blockquote><a href=" + MainProgram.news.Get(result[i].Item1).GetContentURL() + " target='_blank'><font size='2'>Read More>></font></a><font size='2' color='white'>location</font><a href=" + MainProgram.news.Get(result[i].Item1).getGoogleMapsLink( MainProgram.news.Get(result[i].Item1).getLocation()) + " target='_blank'><img src='Map.png' height='40' width='40'></a></blockquote></div></div>");
                            }
                            Response.Write("<br>");
                        }
                    }
                }
                else
                {
                    Response.Write("<div class='row'><p align='center'>Choose One of The Algorithm.</p></div>");
                }
            }

        %>
    </div>

    <div class="container">

        <hr />

        <footer>
            <div class="row">
                <div class="col-lg-12">
                    <p>Copyright &copy; Borborygmutty 2017</p>
                </div>
            </div>
        </footer>

    </div>
    <!-- /.container -->

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="js/modern-business.js"></script>
</body>
</html>
