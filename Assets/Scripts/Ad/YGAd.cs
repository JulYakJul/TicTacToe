using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class YGAds : MonoBehaviour
{
    public static void ShowFullScreenAds(){
        YandexGame.FullscreenShow();
    }

    public static void ShowRewardedAd(int id){
        YandexGame.RewVideoShow(id);
    }
}
