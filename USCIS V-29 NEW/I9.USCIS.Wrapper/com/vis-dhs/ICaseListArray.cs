using System;

namespace com.vis_dhs {

    public interface ICaseListArray {
        
        string GetCaseNumber();
        string GetTypeOfCase();
        //string GetUserField();
        string GetCurrentStateCode();
        string GetMessageCode();
        string GetResolutionCode();
        string GetEligibility();
        string GetResolveDate();

    }

}
