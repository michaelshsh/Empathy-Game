using Constants;
using System.Collections.Generic;
using UnityEngine;
using static Constants.PlayerLabels;

internal class NonRepeatingTextGetter
{
    private List<string> unusedGeneralText = new();
    private List<string> unusedDevReqText = new();
    private List<string> unusedITReqText = new();
    private List<string> unusedPMReqText = new();
    private List<string> unusedHRReqText = new();
    private List<string> unusedQAReqText = new();

    public string NonReapetingTextByLabel(LabelEnum label)
    {
        string res = "";

        switch (label)
        {
            case LabelEnum.Dev:
                if(unusedDevReqText.Count==0)
                {
                    unusedDevReqText = new List<string>(Constants.CardText.DevReqText);
                }
                res = unusedDevReqText[Random.Range(0, unusedDevReqText.Count)];
                unusedDevReqText.Remove(res);
                break;
            case LabelEnum.IT:
                if (unusedITReqText.Count == 0)
                {
                    unusedITReqText = new List<string>(Constants.CardText.ITReqText);
                }
                res = unusedITReqText[Random.Range(0, unusedITReqText.Count)];
                unusedITReqText.Remove(res);
                break;
            case LabelEnum.PM:
                if (unusedPMReqText.Count == 0)
                {
                    unusedPMReqText = new List<string>(Constants.CardText.PMReqText);
                }
                res = unusedPMReqText[Random.Range(0, unusedPMReqText.Count)];
                unusedPMReqText.Remove(res);
                break;
            case LabelEnum.HR:
                if (unusedHRReqText.Count == 0)
                {
                    unusedHRReqText = new List<string>(Constants.CardText.HRReqText);
                }
                res = unusedHRReqText[Random.Range(0, unusedHRReqText.Count)];
                unusedHRReqText.Remove(res);
                break;
            case LabelEnum.QA:
                if (unusedQAReqText.Count == 0)
                {
                    unusedQAReqText = new List<string>(Constants.CardText.QAReqText);
                }
                res = unusedQAReqText[Random.Range(0, unusedQAReqText.Count)];
                unusedQAReqText.Remove(res);
                break;
        }

        return res;
    }

    public string NonReapetingTextGeneral()
    {
        string res = "";

        if (unusedGeneralText.Count == 0)
        {
            unusedGeneralText = new List<string>(Constants.CardText.GeneralText);
        }
        res = unusedGeneralText[Random.Range(0, unusedGeneralText.Count)];
        unusedGeneralText.Remove(res);

        return res;
    }
}