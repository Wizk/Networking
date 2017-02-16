<?php
	include("connection.php");
		
	if($_GET['action'] == "login" && !empty($_POST))
	{
		$email = $_POST["email"];
		$username = $_POST["username"];
		$password = $_POST["password"];
		
		$request = $connection->query("SELECT * FROM `user` WHERE username='$username' AND password='$password'");
		$data = $request->fetch();
		
		if($data)
		{
			echo $data["id"];
		}
	}
	
	if($_GET['action'] == "signin") //&& !empty($_POST))
	{
		$email = $_POST["email"];
		$username = $_POST["username"];
		$password = $_POST["password"];
		
		if (!filter_var($email, FILTER_VALIDATE_EMAIL)) 
		{
			echo("Email is not valid");
			return;
		}
		
		$request = $connection->query("SELECT * FROM `user` WHERE username='$username'");
		$data = $request->fetch();
		
		if($data)
		{
			echo "Username already exist";
			return;
		}
		
		if(strlen($password) < 6)
		{
			echo "Password must contain at least 6 characters";
			return;
		}
		
		echo "1";
		$connection->exec("INSERT INTO user VALUES(NULL, '$email', '$username', '$password')");
	}
?>