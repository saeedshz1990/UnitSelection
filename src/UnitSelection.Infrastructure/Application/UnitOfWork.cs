﻿namespace UnitSelection.Infrastructure.Application;

public interface UnitOfWork
{
    Task Begin();
    Task Complete();
    Task RollBack();
    Task Commit();
}