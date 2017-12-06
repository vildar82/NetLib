﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace NetLib.WPF
{
    public class ValidationTemplate : INotifyDataErrorInfo
    {
        private readonly INotifyPropertyChanged target;
        private readonly IValidator validator;
        private ValidationResult validationResult;
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> validators =
            new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        public ValidationTemplate(INotifyPropertyChanged target)
        {
            this.target = target;
            validator = GetValidator(target.GetType());
            validationResult = validator.Validate(target);
            target.PropertyChanged += Validate;
        }

        public static IValidator GetValidator(Type modelType)
        {
            if (validators.TryGetValue(modelType.TypeHandle, out IValidator validator)) return validator;
            var typeName = $"{modelType.Namespace}.{modelType.Name}Validator";
            var type = modelType.Assembly.GetType(typeName, true);
            validators[modelType.TypeHandle] = validator = (IValidator)Activator.CreateInstance(type);
            return validator;
        }

        private void Validate(object sender, PropertyChangedEventArgs e)
        {
            validationResult = validator.Validate(target);
            foreach (var error in validationResult.Errors)
            {
                RaiseErrorsChanged(error.PropertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult.Errors
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        public bool HasErrors => validationResult.Errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}