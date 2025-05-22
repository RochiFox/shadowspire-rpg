using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            return false;
        }

        currency -= _price;
        return true;
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData _data)
    {
        currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = currency;
    }
}