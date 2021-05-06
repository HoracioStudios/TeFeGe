using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestResponseException: Exception
{
    public int Code { get; }
    public RestResponseException(string _message, int _code): base(_message)
    {
        Code = _code;
    }
}
