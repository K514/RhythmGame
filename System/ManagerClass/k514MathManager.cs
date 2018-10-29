using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514MathManager : MonoBehaviour {

	public static k514MathManager singleton = null;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Math Manager Loaded");
    }

    void Init() {
    }

    // f(x) = x + y_offset;
    public float Funtion_Y_equal_X(float f,float tangent,float y_offset){
        return f*tangent + y_offset;
    }

    // f(x) = x^2 + y_offset
    public float Funtion_Y_equal_Xsqr(float f,float tangent,float y_offset){
        return f*f*tangent + y_offset;
    }

    public int Pow(int v,int n){
        int l_result = 1;
        while(n>0){
            l_result *= v;
            n--;
        }
        return l_result;
    }

}
