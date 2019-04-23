using System;

namespace com.vis_dhs {

    public interface ICaseListArray {
        
        string GetCaseNumber();
        string GetTypeOfCase();
        string GetUserField();
        string GetResponseCode();
        string GetResponseStatement();

        System.DateTime GetResolveDate();

    }

}
