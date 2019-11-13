using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

    //ステータス
    //必ずSetter, Getterとかを使うこと
    [SerializeField] private int life = 3;
    [SerializeField] private int stock = 2;
    [SerializeField] private int power = 0;
    [SerializeField] private int score = 0;

    //上限値
    private int MAX_LIFE = 9;
    private int MAX_STOCK = 9;
    private int MAX_POWER = 128;
    private int MAX_SCORE = 9999999;

    //Reduce
    public int Reduce_Life() {
        life--;
        if(life == 0) {
            GameManager.Instance.Miss();
        }
        return life;
    }

    public int Reduce_Stock() {
        stock--;
        if (stock == 0) {
            GameManager.Instance.Game_Over();
        }
        return stock;
    }    


    //Add
    public void Add_Life() {
        if (life < MAX_LIFE) {
            life++;
        }
    }
    
    public void Add_Stock() {
        if (stock < MAX_SCORE) {
            stock++;
        }
    }

    public void Add_Power() {
        if (power < MAX_POWER) {
            power++;
        }
    }

    public void Add_Score(int value) {
        score += value;
        if (score > MAX_SCORE) {
            score = MAX_SCORE;
        }
    }

    
    //Getter
    public int Get_Life() {
        return life;
    }

    public int Get_Stock() {
        return stock;
    }

    public int Get_Power() {
        return power;
    }

    public int Get_Score() {
        return score;
    }
    

    //Setter
    public void Set_Life(int life) {
        if (life > MAX_LIFE) {
            return;
        }
        if (life >= 0) {
            this.life = life;
        }
        if (life == 0) {
            GameManager.Instance.Miss();
        }
    }

    public void Set_Stock(int stock) {  
        if(stock > MAX_STOCK) {
            return;
        }
        if (stock >= 0) {
            this.stock = stock;
        }
        if (stock == 0) {
            GameManager.Instance.Game_Over();
        }
    }  
    
    public void Set_Power(int power) {
        if(power > MAX_POWER) {
            return;
        }
        if(power < 0) {
            power = 0;
            return;
        }
        this.power = power;
    }

    public void Set_Score(int score) {
        if(score > MAX_SCORE) {
            return;
        }
        if(score < 0) {
            score = 0;
            return;
        }
        this.score = score;
    }

}
