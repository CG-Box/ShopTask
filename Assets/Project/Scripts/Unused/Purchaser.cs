using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class Purchaser : MonoBehaviour
{
    public TMP_Text statusText;

    public void OnPurchaseComplete(Product product)
    {
        switch(product.definition.id)
        {
            case "com.mycompany.iaptutorial.removeads": //"iaptutorial.removeads"
                RemoveAdds();
                break;
            case "com.mycompany.iaptutorial.add500coins": //"iaptutorial.add500coins"
                AddCoins(500);
                break;
            default:
                Debug.Log("product.definition.id: " +product.definition.id);
                break;
        }
    }

    void RemoveAdds()
    {
        Debug.Log("RemoveAdds");
        statusText.text = "RemoveAdds";
    }

    void AddCoins(int coins)
    {
        Debug.Log("AddCoins ${coins}");
        statusText.text = $"AddCoins {coins}";
    }
}
