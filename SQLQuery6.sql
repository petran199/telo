SELECT c.taftotita,c.epwnimo,c.onoma,k.hmerominiaAfixis,k.hmerominiaAnaxwrisis,r.arithmosDwmatiou,
       rt.perigrafi 'typos dwmatiou', rt.[timi portas],e.perifgrafi, e.hmerominiaApo,e.hmerominiaEws,e.timiPortas,
	   t.perigrafi 'tropos plirwmis', datediff(day,k.hmerominiaAfixis,k.hmerominiaAnaxwrisis) 'hmeres kratisis',
	   0 'hmeres normal',
	   0 'hmeres me auxisi'

  from KRATISEIS k
  left join CUSTOMERS c on k.idPelati = c.idPelati
  left join ROOMS r on k.idDwmatiou = r.idDwmatiou
  left join ROOMTYPE rt on r.idDwmatiou = rt.idTyposDwmatiou
  left join EPOXES e on rt.idTyposDwmatiou = e.idTyposDwmatiou
  left join exoflisiKratisis ex on k.idExoflisiKratisis = ex.IdExoflisiKratisis
  left join [GROUP] g on k.idGroup = g.idGroup
  left join TyposKratisis t on k.idTyposKratisis = t.idTyposKratisis


/*
WHERE  ((k.hmerominiaAfixis <= e.hmerominiaApo and k.hmerominiaAnaxwrisis > e.hmerominiaApo)
	     OR (k.hmerominiaAfixis<e.hmerominiaEws AND k.hmerominiaAnaxwrisis>=e.hmerominiaEws)
	     OR (k.hmerominiaAfixis>=e.hmerominiaApo and k.hmerominiaAfixis<=e.hmerominiaEws))
*/