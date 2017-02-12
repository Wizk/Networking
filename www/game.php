<?php
	include("connection.php");
	
	if($_GET['action'] == "creatSession" && !empty($_POST))
	{
		echo "game succed";
		$name = $_POST["name"];
		$ipAddress = $_POST["ipAddress"];
		$hostUser = $_POST["hostUser"];
		$password = $_POST["password"];
		
		$connection->exec("INSERT INTO game VALUES(NULL, '$name', '$ipAddress', '$hostUser', '$password')");
	}
	
	if($_GET['action'] == "listSession")
	{
		$request = $connection->query('SELECT * FROM `game` WHERE 1');
		
		while($data = $request->fetch())
		{
			echo '{';
			echo '"name":"'.$data['name'].'",';
			echo '"ipAddress":"'.$data['ipAddress'].'",';
			echo '"hostUser":"'.$data['hostUser'].'",';
			echo '"password":"'.$data['password'].'"';
			echo '};';
		}
		
	}
?>