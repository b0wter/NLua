function ValidateFoo(foo)
    errors = {}
    if (foo.Name == nil or foo.Name == '') then
        table.insert(errors, "Name darf nicht leer sein!")
    end
    
    return errors;
end

function Transform(foo)
    transformed = TransformedFoo()
    transformed.TransformedName = foo.Name .. " transformed!";
    transformed.TransformedUniqueCustomerId = foo.UniqueCustomerId .. " transformed!";
    transformed.TransformedId = foo.Id * (-1);
    return transformed;
end