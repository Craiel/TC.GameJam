#pragma strict

public var electricArc1 : LineRenderer;


function Start () {

}

function Update () {
electricArc1.sharedMaterial.mainTextureOffset.x = Random.value;

}
