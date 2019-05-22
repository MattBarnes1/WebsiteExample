using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class DevBlogEditorModel : PageModel
    {
        private ApplicationDbContext myContext;
        private UserManager<UserData> myUsers;

   
        [BindProperty]
        public ButtonData[] PossibleSelection { get; set; }
        [BindProperty]
        public string HTMLData {get; set; }
        [BindProperty]
        public string FileName { get; set; }
        [TempData]
        public String ErrorMessage { get; set; }


        public DevBlogEditorModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myContext = myContext;
            this.myUsers = myUsers;

        }



        public bool OnGetFileExists()
        {
            string UserID = myUsers.GetUserId(User);
            return FileEditingService.DoesFileExist("/Home/" + UserID + "/DevBlogs/" + FileName + ".txt");
        }

        public void OnGetOpenFile(string OpenFileName)
        {
            string UserID = myUsers.GetUserId(User);
            String Files = FileEditingService.ReadFileAsString("/Home/" + UserID + "/DevBlogs/" + OpenFileName);
        }

        public JsonResult OnPostGetFiles()
        {           
            string UserID = myUsers.GetUserId(User);
            String[] Files = FileEditingService.GetFilenamesInDirectory("/Home/" + UserID + "/DevBlogs/");
            return new JsonResult(Files);
        }

        public void OnPostSaveToFile()
        {
            if(String.IsNullOrEmpty(HTMLData))
            {
                ErrorMessage = "NO HTML DATA PRESENT!";
                return;
            }
            if (String.IsNullOrEmpty(FileName))
            {
                ErrorMessage = "NO HTML DATA PRESENT!";
                return;
            }
            string UserID = myUsers.GetUserId(User);
            FileEditingService.WriteStringToFile(HTMLData, "/Home/" + UserID + "/DevBlogs/" + FileName + ".txt");
        }

        public class ButtonData
        {
            public String Title { get; set; }
            public String JavascriptFunction { get; set; }
            public String JavascriptOnClickString { get; set; }
            public bool DisplayModal { get; set; }
            public String HTMLModalBody { get; set; }
            public bool isHidden { get; set; }
            public String Name { get; set; }
        }

        public void OnGet()
        {
            PossibleSelection = new ButtonData[] {
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function Insertion(stringToInsert)
                    {
                            if(GridObject.length == 0)
                            {
                                page += stringToInsert;
                            } else {
                                var Grid = GridObject.pop();
                                var Row = Grid.pop();
                                var ColumnPosition = Row.pop();
                                InsertIntoPageString(stringToInsert, ColumnPosition);
                                if(Row.length == 0)
                                {
                                    if(Grid.length == 0)
                                    {
                                        return; //empty column
                                    } else {
                                        GridObject.push(Grid)
                                    }
                                    return;
                                }
                                else
                                {
                                    Grid.push(Row);
                                    GridObject.push(Grid)
                                }
                            }
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    Name = "Set Title",
                    JavascriptFunction =  @"var Title;
                    function SetTitle()
                    {
                       Title = document.getElementById('Title').value;
                        var i = 0;
                    }",
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
                new ButtonData
                {
                    DisplayModal = false,
                    JavascriptFunction =  @"function InsertIntoPageString(Element, aPosition)
                    {
                        var beforeMe = page.substr(0, aPosition);
                        var afterMe = page.substr(aPosition, page.length);
                        page = beforeMe + Element + afterMe;
                    }",
                    isHidden = true
                },
            };

        }
    }
}