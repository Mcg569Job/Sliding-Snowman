using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Hat, Arm }

public class Shop : MonoBehaviour
{

    #region Singleton
    public static Shop instance = null;
    public void Awake()
    {
        instance = this;
    }
    #endregion


    [SerializeField] private Slot[] mSlots, mSlots2;
    private int[] mLock, mLock2;
    private int selectedHat = 0, selectedArm = 0;


    [Header("-ITEMS-")]
    [SerializeField] private GameObject[] hats;
    [SerializeField] private GameObject[] arms, arms2;

    private void Start()
    {
        ChangeDefaultItems();
    }



    public void GetLockStatus()
    {
        mLock = new int[mSlots.Length];
        for (int i = 0; i < mLock.Length; i++)
        {
            if (i == 0) mLock[i] = 1;
            else mLock[i] = PlayerPrefs.HasKey("lock_" + i) ? PlayerPrefs.GetInt("lock_" + i) : 0;

            mSlots[i].Purchased(mLock[i] == 1);
        }

        mLock2 = new int[mSlots2.Length];
        for (int i = 0; i < mLock2.Length; i++)
        {
            if (i == 0) mLock2[i] = 1;
            else mLock2[i] = PlayerPrefs.HasKey("lock2_" + i) ? PlayerPrefs.GetInt("lock2_" + i) : 0;

            mSlots2[i].Purchased(mLock2[i] == 1);
        }

    }
    public void PurchaseHat(int id) => PurchaseItem(ItemType.Hat, id);
    public void PurchaseArm(int id) => PurchaseItem(ItemType.Arm, id);
    private void PurchaseItem(ItemType mType, int id)
    {
        int coin = PlayerPrefs.GetInt("coin");
        if (coin >= mSlots[id].price)
        {
            AudioManager.instance.PlaySound(AT.Purchase);

            switch (mType)
            {
                case ItemType.Hat:
                    PlayerPrefs.SetInt("lock_" + id, 1);
                    mLock[id] = 1;
                    mSlots[id].Purchased(mLock[id] == 1);
                    GameManager.instance.AddCoin(-mSlots[id].price);
                    ChangeItem(mType, id);
                    break;

                case ItemType.Arm:
                    PlayerPrefs.SetInt("lock2_" + id, 1);
                    mLock2[id] = 1;
                    mSlots2[id].Purchased(mLock2[id] == 1);
                    GameManager.instance.AddCoin(-mSlots2[id].price);
                    ChangeItem(mType, id);
                    break;
            }

        }
        else { Debug.Log("yeterli elmas yok"); }
    }

    public void ChangeDefaultItems()
    {
        GetLockStatus();
        selectedHat = PlayerPrefs.GetInt("selectedHat");
        selectedArm = PlayerPrefs.GetInt("selectedArm");
        ChangeItem(ItemType.Hat, selectedHat);
        ChangeItem(ItemType.Arm, selectedArm);
    }

    public void ChangeHat(int id) => ChangeItem(ItemType.Hat, id);
    public void ChangeArm(int id) => ChangeItem(ItemType.Arm, id);
    private void ChangeItem(ItemType mType, int id)
    {
        GetLockStatus();
        
        switch (mType)
        {
            case ItemType.Hat:
                if (mLock[id] != 1) return;

                for (int i = 0; i < mSlots.Length; i++)
                    mSlots[i].Select(false);

                try { mSlots[id].Select(true); } catch { }
                selectedHat = id;

                PlayerPrefs.SetInt("selectedHat", selectedHat);
                break;

            case ItemType.Arm:
                if (mLock2[id] != 1) return;

                for (int i = 0; i < mSlots2.Length; i++)
                    mSlots2[i].Select(false);

                try { mSlots2[id].Select(true); } catch { }
                selectedArm = id;

                PlayerPrefs.SetInt("selectedArm", selectedArm);
                break;

        }
        AudioManager.instance.PlaySound(AT.Click);
        UpdateItem(mType, id);
    }


    private void UpdateItem(ItemType mType, int id)
    {
        switch (mType)
        {
            case ItemType.Hat:
                for (int i = 0; i < hats.Length; i++)
                    hats[i].SetActive(false);

                hats[id].SetActive(true);
                break;
            case ItemType.Arm:

                for (int i = 0; i < arms.Length; i++)
                {
                    arms[i].SetActive(false);
                    arms2[i].SetActive(false);
                }

                arms[id].SetActive(true);
                arms2[id].SetActive(true);
                break;

        }
    }

}
