select idDwmatiou,arithmosDwmatiou 
                                                          from ROOMS
                                                         where idTyposDwmatiou = '2'
                                                           and idDwmatiou
                                                        NOT IN (select idDwmatiou from KRATISEIS 
                                                                 where (hmerominiaAfixis <= '5-12-2015' and hmerominiaAnaxwrisis > '5-12-2015')
                                                                   OR (hmerominiaAfixis<'8-12-2015' AND hmerominiaAnaxwrisis>='8-12-2015')
                                                                   OR (hmerominiaAfixis>='5-12-2015' and hmerominiaAfixis<='8-12-2015'))
                                                            OR (idDwmatiou = '1')