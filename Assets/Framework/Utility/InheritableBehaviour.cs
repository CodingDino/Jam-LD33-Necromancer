// ************************************************************************ 
// File Name:   InheritableBehaviour.cs 
// Purpose:    	
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2014 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Class: InheritableBehaviour
// ************************************************************************ 
public class InheritableBehaviour : MonoBehaviour {
	
	
	// ********************************************************************
	// Function:	Start()
	// Purpose:		Run when new instance of the object is created.
	// ********************************************************************
	void Start () { _Start(); }
	virtual protected void _Start() {}


	// ********************************************************************
	// Function:	OnEnable()
	// Purpose:		Called when the script is enabled.
	// ********************************************************************
	void OnEnable () { _OnEnable(); }
	virtual protected void _OnEnable() {}
	
	
	// ********************************************************************
	// Function:	OnDisable()
	// Purpose:		Called when the script is disabled.
	// ********************************************************************
	void OnDisable () { _OnDisable(); }
	virtual protected void _OnDisable() {}

	
	// ********************************************************************
	// Function:	Update()
	// Purpose:		Called once per frame.
	// ********************************************************************
	void Update () { _Update(); }
	virtual protected void _Update() {}
	
	
	// ********************************************************************
	// Function:	LateUpdate()
	// Purpose:		Called once per frame, after other functions.
	// ********************************************************************
	void LateUpdate () { _LateUpdate(); }
	virtual protected void _LateUpdate() {}
	
	
	// ********************************************************************
	// Function:	OnMouseOver()
	// Purpose:		Called when the user hovers over the collider
	// ********************************************************************
	void OnMouseOver() { _OnMouseOver(); }
	virtual protected void _OnMouseOver() { }
	
	
	// ********************************************************************
	// Function:	OnMouseDown()
	// Purpose:		Called when the user presses the mouse button over this 
	//				collider.
	// ********************************************************************
	void OnMouseDown() { _OnMouseDown(); }
	virtual protected void _OnMouseDown() { }
	
	// ********************************************************************
	// Function:	OnTriggerEnter2D()
	// Purpose:		Called when another trigger enters this one
	// ********************************************************************
	void OnTriggerEnter2D (Collider2D otherCollider) { _OnTriggerEnter2D(otherCollider); }
	virtual protected void _OnTriggerEnter2D (Collider2D otherCollider) { }
}