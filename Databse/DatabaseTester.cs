using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseTester : DatabaseAccess
{
    // Start is called before the first frame update
    void Start()
    {
        Init();

        InsertAccount("nick", "password", "email@mail.com");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
