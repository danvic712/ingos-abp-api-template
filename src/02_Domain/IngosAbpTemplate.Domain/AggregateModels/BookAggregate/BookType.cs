//-----------------------------------------------------------------------
// <copyright file= "BookType.cs">
//     Copyright (c) Danvic.Wang All rights reserved.
// </copyright>
// Author: Danvic.Wang
// Created DateTime: 2021/2/12 21:07:34
// Modified by:
// Description:
//-----------------------------------------------------------------------

namespace IngosAbpTemplate.Domain.AggregateModels.BookAggregate
{
    //public class BookType : Enumeration
    //{
    //    public static BookType Undefined = new BookType(0, nameof(Undefined));

    //    public static BookType Adventure = new BookType(1, nameof(Adventure));

    //    public static BookType Biography = new BookType(2, nameof(Biography));

    //    public static BookType Dystopia = new BookType(3, nameof(Dystopia));

    //    public static BookType Fantastic = new BookType(4, nameof(Fantastic));

    //    public static BookType Horror = new BookType(5, nameof(Horror));

    //    public static BookType Science = new BookType(6, nameof(Science));

    //    public static BookType ScienceFiction = new BookType(7, nameof(ScienceFiction));

    //    public static BookType Poetry = new BookType(8, nameof(Poetry));

    //    public BookType()
    //    {
    //    }

    //    public BookType(int id, string name) : base(id, name)
    //    {
    //    }
    //}

    public enum BookType
    {
        Undefined,
        Adventure,
        Biography,
        Dystopia,
        Fantastic,
        Horror,
        Science,
        ScienceFiction,
        Poetry
    }
}