# BehaviorTree
Basic implementation of behavior tree

# Examples
  That's used for the conetest of Coders of Caribbean in codinggame.com
  Behavior = new Selector(
      new Sequencer(
          new Conditional( IsOnMoving ),
          new Conditional( IsInDangerToBeHit ),
          new BehaviorFunction( AvoidCannon )
      ),
      new Sequencer(
          new Conditional( IsStuck ),
          new Conditional( IsReadyToFire ),
          new Conditional( IsEnemyOnTarget ),
          new BehaviorAction( FireCannon )
      ),
      new Sequencer(
          new Conditional( IsOnMoving ),
          new Conditional( IsReadyToFire ),
          new BehaviorAction( FindNearbyEnemy ),
          new Conditional( IsEnemyOnTarget ),
          new BehaviorAction( FireCannon )
      ),
      new Sequencer(
          new Inverter( IsReadyToFire ),
          new BehaviorAction( ReloadCannon ),
          new Failure()   // to go advance next child of the selector.
      ),
      new Sequencer(
          new BehaviorAction( FindNearbyBarrel ),
          new Conditional( IsNearbyBarrel ),
          new BehaviorFunction( MoveToBarrel )
      ),
      new Sequencer(
          new Inverter( IsNearbyBarrel ),
          new BehaviorAction( FindNearbyEnemy ),
          new Selector(
              new Sequencer(
                  new Conditional( IsStuck ),
                  new Conditional( IsReadyToFire ),
                  new Conditional( IsEnemyOnTarget ),
                  new BehaviorAction( FireCannon )
              ),
              new Sequencer(
                  new Failure(),
                  new Conditional( HasMyShipMoreBarrel ),
                  new BehaviorAction( FindFarthestSpot ),
                  new BehaviorFunction( KeepDistance )
              ),
              new BehaviorFunction( MoveToEnemy )
          )
      ),
      new BehaviorAction( Wait )
    );
