using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyMaxHP = 100;
    public int enemyCurrentHP = 0;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitEnemyHP();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitEnemyHP()
    {
        enemyCurrentHP = enemyMaxHP;
    }





}
