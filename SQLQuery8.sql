SELECT ROOMS.arithmosDwmatiou,ROOMTYPE.perigrafi,ROOMTYPE.idTyposDwmatiou
  FROM ROOMS,ROOMTYPE
 WHERE ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou
 AND ROOMS.idDwmatiou NOT IN(SELECT idDwmatiou FROM KRATISEIS)