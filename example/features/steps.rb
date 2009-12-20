Given /^a scenario$/ do
  
end

When /^it passes$/ do
  
end

Then /^the steps should go green$/ do
  
end

When /^the "([^\"]*)" step is not implemented$/ do |arg1|
  
end

Then /^it should go yellow$/ do
  pending
end

When /^"([^\"]*)" fails$/ do |arg1|
 raise "Error"
end

Then /^it should go red$/ do
  raise "Error"
end