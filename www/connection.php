<?php
	try
	{
		$connection = new PDO('mysql:host=mysql-mafialaw.alwaysdata.net;dbname=mafialaw_mafialaw;charset=utf8', 'mafialaw_root', 'root');
	}
	catch (Exception $e)
	{
		die('Erreur : ' . $e->getMessage());
	}
?>