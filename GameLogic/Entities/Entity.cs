namespace GameLogic.Entities;

using System;
using System.Collections.Generic;
using System.Data;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Entity : ITargeter, ITargetable, IUser
{
    private ITargeter? _targeter;
    private ITargetable? _targetable;
    private IUser? _user;

    public Entity(ITargeter targeter, ITargetable targetable, IUser user)
    {
        this._targeter = targeter;
        this._targetable = targetable;
        this._user = user;
    }

    /*****************
    * IEntity
    ******************/
    public Entity GetEntity() => this;

    /***************************************
    * ITargeter
    ***************************************/
    public bool CanTarget(ITargetable candidate)
    {
        if (this._targeter == null)
            return false;
        return this._targeter.CanTarget(candidate);
    }

    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates)
    {
        if (this._targeter == null)
            return 0;
        return this._targeter.GetMaxTargets(source, candidates);
    }

    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    )
    {
        if (this._targeter == null)
            return new List<ITargetable>();
        return this._targeter.GetEligibleTargets(source, candidates);
    }

    public void ClearTargets()
    {
        if (this._targeter == null)
            return;
        this._targeter.ClearTargets();
    }

    public bool Target(object source, ITargetable target, IEnumerable<ITargetable> candidates)
    {
        if (this._targeter == null)
        {
            throw new NoNullAllowedException("Cannot target without targeter");
        }
        return this._targeter.Target(source, target, candidates);
    }

    public bool Untarget(ITargetable target)
    {
        if (this._targeter == null)
        {
            throw new NoNullAllowedException("Cannot target without targeter");
        }
        return this._targeter.Untarget(target);
    }

    /************************************
    * ITargetable
    ************************************/
    public EFactionRelationship GetRelationTo(ITargeter targeter)
    {
        if (this._targetable == null)
        {
            throw new NoNullAllowedException("Cannot get relation to without targetable");
        }
        return this._targetable.GetRelationTo(targeter);
    }

    /*********************
    * IUser
    *********************/
    public bool CanUse(IUsable usable)
    {
        if (this._user == null)
            return false;
        return this._user.CanUse(usable);
    }

    public IEnumerable<UsableResult> Use(IUsable usable, Entity user, IEnumerable<Entity> targets)
    {
        if (this._user == null)
        {
            throw new NoNullAllowedException("Cannot use without a user");
        }
        return this._user.Use(usable, user, targets);
    }

    public static explicit operator Entity(User v)
    {
        throw new NotImplementedException();
    }

    public static explicit operator Entity(Targeter v)
    {
        throw new NotImplementedException();
    }
}

public interface IEntity
{
    public Entity GetEntity();
}
