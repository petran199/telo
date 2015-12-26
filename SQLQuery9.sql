SELECT count(ROOMS.arithmosDwmatiou)
  FROM ROOMS
 WHERE ROOMS.idTyposDwmatiou='1'
 AND ROOMS.idDwmatiou NOT IN(SELECT idDwmatiou FROM KRATISEIS)
 
