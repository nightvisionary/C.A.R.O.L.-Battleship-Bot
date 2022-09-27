using System;

namespace Battleship
{
    public class SuperCoolAgent : BattleshipAgent
    {
        char[,] attackHistory;
        GridSquare attackGrid;
        GridSquare lastHitSpot;
        bool huntMode;
        Random rng;
        char sweepDir;

        public SuperCoolAgent()
        {
            rng = new Random();
        }

        public override void Initialize()
        {
            attackHistory = new char[10, 10];
            attackGrid = new GridSquare();
            attackGrid.x = 5;
            attackGrid.y = 5;
            lastHitSpot = new GridSquare();
            huntMode = true;
            sweepDir = '\0';
        }

        public override string ToString()
        {
            return $"Battleship Agent '{GetNickname()}'";
        }

        public override string GetNickname()
        {
            return "C.A.R.O.L.";
        }
        public override void SetOpponent(string opponent)
        {
            return;
        }

        public override GridSquare LaunchAttack()
        {
            if (huntMode == false &&sweepDir == 'N')
            {
                attackGrid.y += 1;
            }
            else if(huntMode == false && sweepDir == 'S')
            {
                attackGrid.y -= 1;
            }
            else if(huntMode == false && sweepDir == 'E')
            {
                attackGrid.x -= 1;
            }
            else if(huntMode == false && sweepDir == 'W')
            {
                attackGrid.x += 1;
            }
            else
            {
                while (attackHistory[attackGrid.x, attackGrid.y] != '\0')
                {
                    attackGrid.x = rng.Next(10);
                    attackGrid.y = rng.Next(10);
                }
            }
            return attackGrid;
        }

        public override void DamageReport(char report)
        {
            //case:every ship is missed
            if ( report == '\0')
            {
                attackHistory[attackGrid.x,attackGrid.y] = 'x';
                if(!huntMode)
                {
                    //SWITCH TO SOUTH SWEEP (we're yelling this)
                    if((sweepDir =='N') && //previous direction
                        lastHitSpot.y>0 && //still on board
                        attackHistory[lastHitSpot.x,lastHitSpot.y-1]=='\0')//havent shot here yet
                    {
                        sweepDir = 'S';
                        attackGrid = lastHitSpot;
                    }
                    //switch to east sweep
                    else if ((sweepDir == 'N'||sweepDir =='S') && //previous direction
                        lastHitSpot.x > 0 && //still on board
                        attackHistory[lastHitSpot.x-1, lastHitSpot.y] == '\0')//havent shot here yet
                    {
                        sweepDir = 'E';
                        attackGrid = lastHitSpot;
                    }

                    else if ((sweepDir == 'N' || sweepDir == 'S'|| sweepDir =='E') && //previous direction
                        lastHitSpot.x <9 && //still on board
                        attackHistory[lastHitSpot.x +1, lastHitSpot.y] == '\0')//havent shot here yet
                    {
                        sweepDir = 'W';
                        attackGrid = lastHitSpot;
                    }
                    else
                    {
                        huntMode = true;
                        lastHitSpot = new GridSquare();
                        sweepDir = '\0';
                    }
                }
            }

            //case:ship hit
            else
            {
                attackHistory[attackGrid.x, attackGrid.y] = 'h';
                if (huntMode)
                {
                    huntMode = false;
                    lastHitSpot = attackGrid;
                    sweepDir = 'N';
                }
                if(sweepDir =='N'&&
                    attackGrid.y<9&&
                    attackHistory[attackGrid.x, attackGrid.y+1]=='\0')
                {
                    sweepDir = 'N';
                }
                else if ((sweepDir == 'N' || sweepDir == 'S')&&
                    attackGrid.y >0 &&
                    attackHistory[attackGrid.x, attackGrid.y - 1] == '\0')
                {
                    sweepDir = 'S';
                }
                else if ((sweepDir == 'N' || sweepDir == 'S' || sweepDir =='E')&&
                    attackGrid.x >0 &&
                    attackHistory[attackGrid.x-1, attackGrid.y] == '\0')
                {
                    sweepDir = 'E';
                }
                else if ((sweepDir == 'N' || sweepDir == 'S' || sweepDir == 'E' ||sweepDir =='W') &&
                    attackGrid.x <9 &&
                    attackHistory[attackGrid.x +1, attackGrid.y] == '\0')
                {
                    sweepDir = 'W';
                }
                else
                {
                    huntMode = true;
                    lastHitSpot = new GridSquare();
                    sweepDir = '\0';
                }
            }
        }

        public override BattleshipFleet PositionFleet()
        {
            BattleshipFleet myFleet = new BattleshipFleet();
            int FleetSetup = rng.Next(5);
            if (FleetSetup == 0)
            {
                myFleet.Carrier = new ShipPosition(4, 4, ShipRotation.Vertical);
                myFleet.Battleship = new ShipPosition(1, 1, ShipRotation.Horizontal);
                myFleet.Destroyer = new ShipPosition(7, 4, ShipRotation.Vertical);
                myFleet.Submarine = new ShipPosition(9, 1, ShipRotation.Vertical);
                myFleet.PatrolBoat = new ShipPosition(8, 9, ShipRotation.Horizontal);
            }
            else if (FleetSetup == 1)
            {
                myFleet.Carrier = new ShipPosition(0, 0, ShipRotation.Horizontal);
                myFleet.Battleship = new ShipPosition(3, 4, ShipRotation.Vertical);
                myFleet.Destroyer = new ShipPosition(6, 2, ShipRotation.Vertical);
                myFleet.Submarine = new ShipPosition(5, 8, ShipRotation.Horizontal);
                myFleet.PatrolBoat = new ShipPosition(8, 6, ShipRotation.Horizontal);
            }
            else if (FleetSetup == 2)
            {
                myFleet.Carrier = new ShipPosition(3, 7, ShipRotation.Horizontal);
                myFleet.Battleship = new ShipPosition(1, 1, ShipRotation.Vertical);
                myFleet.Destroyer = new ShipPosition(4, 3, ShipRotation.Horizontal);
                myFleet.Submarine = new ShipPosition(8, 0, ShipRotation.Vertical);
                myFleet.PatrolBoat = new ShipPosition(1, 9, ShipRotation.Horizontal);
            }
            else if (FleetSetup == 3)
            {
                myFleet.Carrier = new ShipPosition(9, 0, ShipRotation.Vertical);
                myFleet.Battleship = new ShipPosition(0, 3, ShipRotation.Vertical);
                myFleet.Destroyer = new ShipPosition(4, 4, ShipRotation.Horizontal);
                myFleet.Submarine = new ShipPosition(7,7, ShipRotation.Horizontal);
                myFleet.PatrolBoat = new ShipPosition(4, 0, ShipRotation.Vertical);
            }
            else if (FleetSetup == 4)
            {
                myFleet.Carrier = new ShipPosition(3,3, ShipRotation.Horizontal);
                myFleet.Battleship = new ShipPosition(4,7, ShipRotation.Horizontal);
                myFleet.Destroyer = new ShipPosition(0, 4, ShipRotation.Vertical);
                myFleet.Submarine = new ShipPosition(9, 1, ShipRotation.Vertical);
                myFleet.PatrolBoat = new ShipPosition(9,8, ShipRotation.Vertical);
            }
            else
            {
                myFleet.Carrier = new ShipPosition(0, 0, ShipRotation.Vertical);
                myFleet.Battleship = new ShipPosition(2, 0, ShipRotation.Vertical);
                myFleet.Destroyer = new ShipPosition(4, 0, ShipRotation.Vertical);
                myFleet.Submarine = new ShipPosition(6, 0, ShipRotation.Vertical);
                myFleet.PatrolBoat = new ShipPosition(8, 0, ShipRotation.Horizontal);
            }

            return myFleet;
        }
    }
}
