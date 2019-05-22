using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication5.Data;

namespace WebApplication5.Areas.Identity.Pages.Application
{
    public class RequestOfFeatureModel : PageModel
    {
        private UserManager<UserData> myUsers;
        private ApplicationDbContext myContext;

        public RequestOfFeatureModel(ApplicationDbContext myContext, UserManager<UserData> myUsers)
        {
            this.myUsers = myUsers;
            this.myContext = myContext;
        }

        [BindProperty]
        public Request[] CurrentRequests { get; set; }

        [BindProperty]
        public String NewRequest { get; set; }

        [BindProperty]
        public List<SelectListItem> myChoiceSelections { get; set; }

        [BindProperty]
        public List<SelectListItem> myFilterSelections { get; set; }
        public string Error { get; private set; }

        public JsonResult OnGetFilterType(String id)
        {
            if(String.IsNullOrEmpty(id))
            {
                Error = "You submitted an empty report.";
                return new JsonResult(null);
            }
            if (id.CompareTo("0") == 0)
            {
                CurrentRequests = myContext.myRequests.Where(delegate (Request D)
                {
                    return D.myType == RequestType.BUG_REPORT;
                }).ToArray();
            }
            else if (id.CompareTo("1") == 0)
            {
                CurrentRequests = myContext.myRequests.Where(delegate (Request D)
                {
                    return D.myType == RequestType.FEATURE_REQUEST;
                }).ToArray();
            }
            return new JsonResult(CurrentRequests);
        }


        public void OnPostNewRequestFeature(String ReportType)
        {
            Request myRequest = new Request();
            if(ReportType == null)
            {
                Error = "You submitted an empty report.";
                return;
            }
            if(ReportType.CompareTo("0") == 0 && !String.IsNullOrEmpty(NewRequest)) //Bug Report
            {
                myRequest.RequestersID = myUsers.GetUserId(User);
                myRequest.RequestBody = NewRequest;
                myRequest.myType = RequestType.BUG_REPORT;
                NewRequest = "";
                myContext.myRequests.Add(myRequest);
                myContext.SaveChanges();
                
            }
            else if(ReportType.CompareTo("1") == 0 && !String.IsNullOrEmpty(NewRequest))//Feature
            {
                myRequest.RequestersID = myUsers.GetUserId(User);
                myRequest.RequestBody = NewRequest;
                myRequest.myType = RequestType.FEATURE_REQUEST;
                NewRequest = "";
                myContext.myRequests.Add(myRequest);
                myContext.SaveChanges();
            }
            else if(!String.IsNullOrEmpty(NewRequest))
            {
                Error = "Invalid Selection for feature request.";
            }
        }

        public void OnGet()
        {
            myFilterSelections = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Bug Report",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "Feature Request",
                    Value = "1"
                },
            };
            myChoiceSelections = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Bug Report",
                    Value = "0"                    
                },
                new SelectListItem
                {
                    Text = "Feature Request",
                    Value = "1"
                },
            };
        }


    }
}