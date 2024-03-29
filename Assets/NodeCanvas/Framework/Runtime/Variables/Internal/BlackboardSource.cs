﻿using System;
using System.Collections.Generic;
using System.Linq;
using ParadoxNotion;
using UnityEngine;


namespace NodeCanvas.Framework.Internal{

	/// Blackboard holds Variable and is able to save and load itself. It's usefull for interop
	/// communication within the program, saving and loading systems etc. This is the main implementation class of IBlackboard and the one
	/// being serialized.
	[Serializable]
	sealed public class BlackboardSource : IBlackboard {

		[SerializeField]
		private string _name;
		[SerializeField]
		private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();


		public string name{
			get {return _name;}
			set {_name = value;}
		}

		public Dictionary<string, Variable> variables{
			get {return _variables;}
			set {_variables = value;}
		}

		public GameObject propertiesBindTarget{
			get {return null;}
		}

		///An indexer to access variables on the blackboard. It's recomended to use GetValue<T> instead
		public object this[string varName]{
			get
			{
				try {return variables[varName].value;}
				catch {return null;}
			}
			set
			{
				SetValue(varName, value);
			}
		}

		//required
		public BlackboardSource(){}

		//Initialize variables data binding for the target game object
		public void InitializePropertiesBinding(GameObject targetGO, bool callSetter){
			foreach (var data in variables.Values)
				data.InitializePropertyBinding(targetGO, callSetter);
		}

		///Adds a new Variable in the blackboard
		public Variable AddVariable(string varName, object value){
			
			if (value == null){
				Debug.LogError("<b>Blackboard:</b> You can't use AddVariable with a null value. Use AddVariable(string, Type) to add the new data first");
				return null;
			}
			
			var newData = AddVariable(varName, value.GetType());
			if (newData != null)
				newData.value = value;

			return newData;
		}

		///Adds a new Variable in the blackboard defining name and type instead of value
		public Variable AddVariable(string varName, Type type){

			if (variables.ContainsKey(varName)){
				var existing = GetVariable(varName, type);
				if (existing == null){
					Debug.LogError(string.Format("<b>Blackboard:</b> Variable with name '{0}' already exists in blackboard '{1}', but is of different type!. Returning null instead of new", varName, this.name));
				} else {
					Debug.LogWarning(string.Format("<b>Blackboard:</b> Variable with name '{0}' already exists in blackboard '{1}'. Returning existing instead of new", varName, this.name));
				}
				return existing;
			}

			var dataType = typeof(Variable<>).MakeGenericType(new Type[]{type});
			var newData = (Variable)Activator.CreateInstance(dataType);
			newData.name = varName;
			variables[varName] = newData;
			return newData;
		}

		///Deletes the Variable of name provided regardless of type
		public void DeleteVariable(string varName){
			variables.Remove(varName);
		}

		///Gets the variable data value from the blackboard with provided name and type T
		public T GetValue<T>(string varName){

			//Try for same T first else get it as an object type value
			try {return (variables[varName] as Variable<T>).value; }
			catch
			{
				try { return (T)variables[varName].value; }
				catch
				{
					if (!variables.ContainsKey(varName)){
						Debug.LogError(string.Format("<b>Blackboard:</b> No Variable of name '{0}' and type '{1}' exists on Blackboard '{2}'. Returning default T...", varName, typeof(T).FriendlyName(), this.name));
						return default(T);
					}
				}
			}
			Debug.LogError(string.Format("<b>Blackboard:</b> Can't cast value of variable with name '{0}' to type '{1}'", varName, typeof(T).FriendlyName() ));
			return default(T);
		}

		///Set the value of the Variable variable defined by its name. If a data by that name and type doesnt exist, a new data is added by that name
		public Variable SetValue(string varName, object value){

			//this can fail either cause there is no Key with that name or cause of casting
			try
			{
				var varData = variables[varName];
				varData.value = value;
				return varData;
			}
			catch
			{
				//in case of Key, we add a new data
				if (!variables.ContainsKey(varName)){
					Debug.Log(string.Format("<b>Blackboard:</b> No Variable of name '{0}' and type '{1}' exists on Blackboard '{2}'. Adding new instead...", varName, value != null? value.GetType().FriendlyName() : "null", this.name));
					return AddVariable(varName, value);				
				}
			}
			//in case of casting error inform the user
			Debug.LogError(string.Format("<b>Blackboard:</b> Can't cast value '{0}' to blackboard variable of name '{1}' and type '{2}'", value != null? value.ToString() : "null", varName, variables[varName].varType.Name ));
			return null;
		}

		///Gets the Variable object of a certain name and optional specified type
		public Variable GetVariable(string varName, Type ofType = null){
			try
			{
				var data = variables[varName];
				if (ofType == null || ofType.RTIsAssignableFrom(data.varType))
					return data;				
				return null;
			}
			catch {	return null; }
		}

		///Generic verison of GetVariable
		public Variable GetVariable<T>(string varName){
			return GetVariable(varName, typeof(T));
		}

		///Get all data names of the blackboard
		public string[] GetVariableNames(){
			return variables.Keys.ToArray();
		}

		///Get all data names of the blackboard and of specified type
		public string[] GetVariableNames(Type ofType){
			return variables.Values.Where(v => (ofType).RTIsAssignableFrom(v.varType)).Select(v => v.name).ToArray();
		}


		//GENERIC VERSIONS...

		///Adds a new Variable<T> with provided value and returns it.
		public Variable<T> AddVariable<T>(string varName, T value){
			var data = AddVariable<T>(varName);
			data.value = value;
			return data;
		}

		///Adds a new Variable<T> with default T value and returns it
		public Variable<T> AddVariable<T>(string varName){
			return (Variable<T>)AddVariable(varName, typeof(T));
		}
	}
}