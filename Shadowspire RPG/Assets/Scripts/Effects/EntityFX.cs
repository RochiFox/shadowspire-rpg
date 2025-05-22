using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player Player;
    protected SpriteRenderer Sr;

    [Header("Pop Up Text")] [SerializeField]
    private GameObject popUpTextPrefab;

    [Header("Flash FX")] [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Ailment colors")] [SerializeField]
    private Color[] igniteColor;

    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment particles")] [SerializeField]
    private ParticleSystem igniteFx;

    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")] [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    private GameObject myHealthBar;

    protected virtual void Start()
    {
        Sr = GetComponentInChildren<SpriteRenderer>();
        Player = PlayerManager.instance.player;

        originalMat = Sr.material;

        myHealthBar = GetComponentInChildren<HealthBarUI>(true).gameObject;
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1.5f, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            Sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            Sr.color = Color.white;
        }
    }

    protected IEnumerator FlashFX()
    {
        Sr.material = hitMat;
        Color currentColor = Sr.color;
        Sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        Sr.color = currentColor;
        Sr.material = originalMat;
    }

    protected void RedColorBlink()
    {
        Sr.color = Sr.color != Color.white ? Color.white : Color.red;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        Sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();

        InvokeRepeating(nameof(IgniteColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();
        InvokeRepeating(nameof(ChillColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();
        InvokeRepeating(nameof(ShockColorFx), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void IgniteColorFx()
    {
        Sr.color = Sr.color != igniteColor[0] ? igniteColor[0] : igniteColor[1];
    }

    private void ChillColorFx()
    {
        Sr.color = Sr.color != chillColor[0] ? chillColor[0] : chillColor[1];
    }

    private void ShockColorFx()
    {
        Sr.color = Sr.color != shockColor[0] ? shockColor[0] : shockColor[1];
    }

    public void CreateHitFx(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_critical)
        {
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition),
            Quaternion.identity);
        newHitFx.transform.Rotate(hitFxRotation);
        Destroy(newHitFx, .5f);
    }
}