<?php
	include("connection.php");
		
	if($_GET['action'] == "listItems") //&& !empty($_POST))
	{		
		$request = $connection->query("SELECT * FROM `item`");
		
		while($data = $request->fetch())
		{
			echo '{';
			echo '"id":"'.$data['id'].'",';
			echo '"name":"'.$data['name'].'",';
			echo '"category":"'.$data['category'].'",';
			echo '"cost":"'.$data['cost'].'",';
			echo '"sale":"'.$data['sale'].'",';
			echo '"payable":"'.$data['payable'].'",';
			echo '"stock":"'.$data['stock'].'"';
			echo '};';
		}
	}
	
	if($_GET['action'] == "buyItem") //&& !empty($_POST))
	{
		$idUser = $_POST["idUser"];
		$idItem = $_POST["idItem"];
		
		$connection->exec("INSERT INTO userItem VALUES('$idUser', '$idItem', '$password')");
		
		$request = $connection->query("SELECT * FROM `user` WHERE idUser='$idUser'");
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