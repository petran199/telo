select k.hmerominiaAfixis,k.hmerominiaAnaxwrisis,e.hmerominiaApo,e.hmerominiaEws,
case 
when k.hmerominiaAfixis between e.hmerominiaApo and e.hmerominiaEws
then 'kati'
end rofl

from KRATISEIS k , EPOXES e

SELECT k.idDwmatiou, k.idKratisis,k.hmerominiaAfixis,k.hmerominiaAnaxwrisis,e.perifgrafi 'periodos',e.hmerominiaApo,e.hmerominiaEws
 FROM KRATISEIS k,EPOXES e
  WHERE ((k.hmerominiaAfixis <= e.hmerominiaApo and k.hmerominiaAnaxwrisis > e.hmerominiaApo)
                                                            OR (k.hmerominiaAfixis<e.hmerominiaEws AND k.hmerominiaAnaxwrisis>=e.hmerominiaEws)
                                                            OR (k.hmerominiaAfixis>=e.hmerominiaApo and k.hmerominiaAfixis<=e.hmerominiaEws))
and e.idTyposDwmatiou=1