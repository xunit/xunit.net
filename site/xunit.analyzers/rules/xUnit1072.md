---
analyzer: true
title: xUnit1072
description: Test methods from referenced assemblies are not supported in Native AOT
severity: Warning
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a Native AOT test class inherits test methods from a binary reference.

## Reason for rule

The source generators for Native AOT-mode require access to the source for test methods, and cannot operate purely on reflection.

## How to fix violations

To fix a violation of this rule, move the test method to source.
